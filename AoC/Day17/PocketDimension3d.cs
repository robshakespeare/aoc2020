using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AoC.Day17
{
    public class PocketDimension3d
    {
        private const char Active = '#';

        public IReadOnlySet<Coords3d> ActiveCubes { get; }

        public PocketDimension3d(string input) =>
            ActiveCubes = input.ReadLines().SelectMany(
                    (line, y) => line.Select((chr, x) => (chr, x))
                        .Where(cube => cube.chr == Active)
                        .Select(cube => new Coords3d(cube.x, y, 0)))
                .ToImmutableHashSet();

        private PocketDimension3d(IReadOnlySet<Coords3d> activeCubes) =>
            ActiveCubes = activeCubes;

        public static PocketDimension3d Run(PocketDimension3d pocketDimension, int numGenerations)
        {
            for (var gen = 0; gen < numGenerations; gen++)
            {
                pocketDimension = pocketDimension.NextGeneration();
            }

            return pocketDimension;
        }

        public PocketDimension3d NextGeneration()
        {
            const int expand = 1;
            var bounds = CalculateBounds();
            List<Coords3d> newActiveCubes = new();

            for (var z = bounds.Z.Min - expand; z <= bounds.Z.Max + expand; z++)
            {
                for (var x = bounds.X.Min - expand; x <= bounds.X.Max + expand; x++)
                {
                    for (var y = bounds.Y.Min - expand; y <= bounds.Y.Max + expand; y++)
                    {
                        var cubeCoords = new Coords3d(x, y, z);
                        var isActive = ActiveCubes.Contains(cubeCoords);
                        var activeNeighbors = GetActiveNeighbors(cubeCoords).Count();

                        var newActiveState = isActive
                            ? activeNeighbors == 2 || activeNeighbors == 3
                            : activeNeighbors == 3;

                        if (newActiveState)
                        {
                            newActiveCubes.Add(cubeCoords);
                        }
                    }
                }
            }

            return new PocketDimension3d(newActiveCubes.ToImmutableHashSet());
        }

        private Bounds3d CalculateBounds()
        {
            var xMin = 0;
            var xMax = 0;
            var yMin = 0;
            var yMax = 0;
            var zMin = 0;
            var zMax = 0;

            foreach (var (x, y, z) in ActiveCubes)
            {
                xMin = Math.Min(xMin, x);
                xMax = Math.Max(xMax, x);

                yMin = Math.Min(yMin, y);
                yMax = Math.Max(yMax, y);

                zMin = Math.Min(zMin, z);
                zMax = Math.Max(zMax, z);
            }

            return new Bounds3d(
                (xMin, xMax),
                (yMin, yMax),
                (zMin, zMax));
        }

        private static IEnumerable<Coords3d> GetAllDirections() =>
            Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .Select(z => new Coords3d(x, y, z))));

        private static readonly IReadOnlyCollection<Coords3d> Directions = GetAllDirections()
            .Where(dir => !(dir.X == 0 && dir.Y == 0 && dir.Z == 0))
            .ToArray();

        private IEnumerable<Coords3d> GetActiveNeighbors(Coords3d coords) =>
            Directions
                .Select(dir => new Coords3d(
                    coords.X + dir.X,
                    coords.Y + dir.Y,
                    coords.Z + dir.Z))
                .Where(position => ActiveCubes.Contains(position));
    }

    public record Coords3d(int X, int Y, int Z)
    {
    }

    public record Bounds3d((int Min, int Max) X, (int Min, int Max) Y, (int Min, int Max) Z)
    {
    }
}
