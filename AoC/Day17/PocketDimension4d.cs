using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AoC.Day17
{
    public class PocketDimension4d
    {
        private const char Active = '#';

        public IReadOnlySet<Coords4d> ActiveCubes { get; }

        public PocketDimension4d(string input) =>
            ActiveCubes = input.ReadLines().SelectMany(
                    (line, y) => line.Select((chr, x) => (chr, x))
                        .Where(cube => cube.chr == Active)
                        .Select(cube => new Coords4d(cube.x, y, 0, 0)))
                .ToImmutableHashSet();

        private PocketDimension4d(IReadOnlySet<Coords4d> activeCubes) =>
            ActiveCubes = activeCubes;

        public static PocketDimension4d Run(PocketDimension4d pocketDimension, int numGenerations)
        {
            for (var gen = 0; gen < numGenerations; gen++)
            {
                pocketDimension = pocketDimension.NextGeneration();
            }

            return pocketDimension;
        }

        public PocketDimension4d NextGeneration()
        {
            const int expand = 1;
            var bounds = CalculateBounds();
            List<Coords4d> newActiveCubes = new();

            for (var z = bounds.Z.Min - expand; z <= bounds.Z.Max + expand; z++)
            {
                for (var x = bounds.X.Min - expand; x <= bounds.X.Max + expand; x++)
                {
                    for (var y = bounds.Y.Min - expand; y <= bounds.Y.Max + expand; y++)
                    {
                        for (var w = bounds.W.Min - expand; w <= bounds.W.Max + expand; w++)
                        {
                            var cubeCoords = new Coords4d(x, y, z, w);
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
            }

            return new PocketDimension4d(newActiveCubes.ToImmutableHashSet());
        }

        private Bounds4d CalculateBounds()
        {
            var xMin = 0;
            var xMax = 0;
            var yMin = 0;
            var yMax = 0;
            var zMin = 0;
            var zMax = 0;
            var wMin = 0;
            var wMax = 0;

            foreach (var (x, y, z, w) in ActiveCubes)
            {
                xMin = Math.Min(xMin, x);
                xMax = Math.Max(xMax, x);

                yMin = Math.Min(yMin, y);
                yMax = Math.Max(yMax, y);

                zMin = Math.Min(zMin, z);
                zMax = Math.Max(zMax, z);

                wMin = Math.Min(wMin, w);
                wMax = Math.Max(wMax, w);
            }

            return new Bounds4d(
                (xMin, xMax),
                (yMin, yMax),
                (zMin, zMax),
                (wMin, wMax));
        }

        private static IEnumerable<Coords4d> GetAllDirections() =>
            Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .SelectMany(z => Enumerable.Range(-1, 3)
                            .Select(w => new Coords4d(x, y, z, w)))));

        public static readonly IReadOnlyCollection<Coords4d> Directions = GetAllDirections()
            .Where(dir => !(dir.X == 0 && dir.Y == 0 && dir.Z == 0 && dir.W == 0))
            .ToArray();

        private IEnumerable<Coords4d> GetActiveNeighbors(Coords4d coords) =>
            Directions
                .Select(dir => new Coords4d(
                    coords.X + dir.X,
                    coords.Y + dir.Y,
                    coords.Z + dir.Z,
                    coords.W + dir.W))
                .Where(position => ActiveCubes.Contains(position));

        public record Coords4d(int X, int Y, int Z, int W)
        {
        }

        public record Bounds4d((int Min, int Max) X, (int Min, int Max) Y, (int Min, int Max) Z, (int Min, int Max) W)
        {
        }
    }
}
