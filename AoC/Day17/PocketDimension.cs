using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AoC.Day17
{
    public class PocketDimension
    {
        private const char Active = '#';
        //private const char Inactive = '.';
        //private readonly IReadOnlyDictionary<(int x, int y, int z), char>

        public IReadOnlySet<CubeCoords> ActiveCubes { get; }
        public Bounds Bounds { get; }

        public PocketDimension(string input)
        {
            ActiveCubes = input.ReadLines().SelectMany(
                (line, y) => line.Select((chr, x) => (chr, x))
                    .Where(cube => cube.chr == Active)
                    .Select(cube => new CubeCoords(cube.x, y, 0)))
                .ToImmutableHashSet();

            var xMin = 0;
            var xMax = 0;
            var yMin = 0;
            var yMax = 0;

            foreach (var (x, y, _) in ActiveCubes)
            {
                xMin = Math.Min(xMin, x);
                xMax = Math.Max(xMax, x);
                yMin = Math.Min(yMin, y);
                yMax = Math.Max(yMax, y);
            }

            Bounds = new Bounds(
                (xMin, xMax),
                (yMin, yMax),
                (0,0));
        }

        private PocketDimension(IReadOnlySet<CubeCoords> activeCubes, Bounds bounds)
        {
            ActiveCubes = activeCubes;
            Bounds = bounds;
        }

        public static PocketDimension Run(PocketDimension pocketDimension, int numGenerations)
        {
            for (var genIndex = 0; genIndex < numGenerations; genIndex++)
            {
                pocketDimension = pocketDimension.NextGeneration(genIndex);
            }

            return pocketDimension;
        }

        public PocketDimension NextGeneration(int genIndex)
        {
            var expand = genIndex + 1;

            var xMin = 0;
            var xMax = 0;
            var yMin = 0;
            var yMax = 0;
            var zMin = 0;
            var zMax = 0;

            List<CubeCoords> newActiveCubes = new();

            for (var z = Bounds.Z.Min - expand; z <= Bounds.Z.Max + expand; z++)
            {
                for (var x = Bounds.X.Min - expand; x <= Bounds.X.Max + expand; x++)
                {
                    for (var y = Bounds.Y.Min - expand; y <= Bounds.Y.Max + expand; y++)
                    {
                        var cubeCoords = new CubeCoords(x, y, z);
                        var isActive = ActiveCubes.Contains(cubeCoords);
                        var activeNeighbors = GetActiveNeighbors(cubeCoords).Count();

                        var newActiveState = isActive
                            ? activeNeighbors == 2 || activeNeighbors == 3
                            : activeNeighbors == 3;

                        if (newActiveState)
                        {
                            newActiveCubes.Add(cubeCoords);
                            xMin = Math.Min(xMin, x);
                            xMax = Math.Max(xMax, x);
                            yMin = Math.Min(yMin, y);
                            yMax = Math.Max(yMax, y);
                            zMin = Math.Min(yMin, z);
                            zMax = Math.Max(yMax, z);
                        }
                    }
                }
            }

            return new PocketDimension(
                newActiveCubes.ToImmutableHashSet(),
                new Bounds((xMin, xMax), (yMin, yMax), (zMin, zMax)));
        }

        private static IEnumerable<CubeCoords> GetAllDirections() =>
            Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .Select(z => new CubeCoords(x, y, z))));

        private static readonly IReadOnlyCollection<CubeCoords> Directions = GetAllDirections()
            .Where(dir => !(dir.X == 0 && dir.Y == 0 && dir.Z == 0))
            .ToArray();

        private IEnumerable<CubeCoords> GetActiveNeighbors(CubeCoords cubeCoords) =>
            Directions
                .Select(dir => new CubeCoords(
                    cubeCoords.X + dir.X,
                    cubeCoords.Y + dir.Y,
                    cubeCoords.Z + dir.Z))
                .Where(position => ActiveCubes.Contains(position));

        //private static IEnumerable<Vector3> GetAllDirections()
        //{
        //    return Enumerable.Range(-1, 3)
        //        .SelectMany(x => Enumerable.Range(-1, 3)
        //            .SelectMany(y => Enumerable.Range(-1, 3)
        //                .Select(z => new Vector3(x, y, z))));
        //}

        //private static readonly IReadOnlyCollection<Vector3> Directions = GetAllDirections().Where(dir => dir != new Vector3(0, 0, 0)).ToArray();

        //private int GetActiveNeighbors(CubeCoords cubeCoords)
        //{
        //    var center = new Vector3(cubeCoords.X, cubeCoords.Y, cubeCoords.Z);
        //    return Directions
        //        .Select(dir => center + dir)
        //        .Select(position =>
        //        {
        //            var otherCubeCoords = new CubeCoords(position.X.Round(), position.Y.Round(), position.Z.Round());
        //            return ActiveCubes.Contains(otherCubeCoords) ? 1 : 0;
        //        })
        //        .Sum();
        //}
    }

    public record CubeCoords(int X, int Y, int Z)
    {
    }

    public record Bounds((int Min, int Max) X, (int Min, int Max) Y, (int Min, int Max) Z)
    {
    }
}
