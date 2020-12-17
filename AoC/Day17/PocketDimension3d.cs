using System.Collections.Generic;
using System.Linq;

namespace AoC.Day17
{
    public class PocketDimension3d
    {
        private const char Active = '#';

        public IReadOnlyList<IReadOnlyList<IReadOnlyList<bool>>> Cubes { get; }

        public PocketDimension3d(string input) =>
            Cubes = new[]
            {
                input.ReadLines().Select(
                        line => line
                            .Select(cube => cube == Active)
                            .ToArray())
                    .ToArray()
            };

        private PocketDimension3d(IReadOnlyList<IReadOnlyList<IReadOnlyList<bool>>> cubes) =>
            Cubes = cubes;

        public static PocketDimension3d Run(PocketDimension3d pocketDimension, int numGenerations)
        {
            for (var gen = 0; gen < numGenerations; gen++)
            {
                pocketDimension = pocketDimension.NextGeneration();
            }

            return pocketDimension;
        }

        public long CountActiveCubes() => Cubes.SelectMany(layer => layer.SelectMany(row => row)).Count(active => active);

        public PocketDimension3d NextGeneration()
        {
            const int expand = 1;

            var newZLength = Cubes.Count + expand * 2;
            var newYLength = Cubes[0].Count + expand * 2;
            var newXLength = Cubes[0][0].Count + expand * 2;

            var layers = new bool[newZLength][][];

            for (var z = 0; z < newZLength; z++)
            {
                var rows = new bool[newYLength][];
                layers[z] = rows;

                for (var y = 0; y < newYLength; y++)
                {
                    var cols = new bool[newXLength];
                    rows[y] = cols;

                    for (var x = 0; x < newXLength; x++)
                    {
                        var oldCoords = new Coords3d(x - 1, y - 1, z - 1);

                        var isActive = IsCubeActive(oldCoords);
                        var activeNeighbors = GetActiveNeighbors(oldCoords).Count();

                        var newActiveState = isActive
                            ? activeNeighbors == 2 || activeNeighbors == 3
                            : activeNeighbors == 3;

                        cols[x] = newActiveState;
                    }
                }
            }

            return new PocketDimension3d(layers);
        }

        private bool IsCubeActive(Coords3d coords)
        {
            var (x, y, z) = coords;

            if ((z < 0 || z >= Cubes.Count) ||
                (y < 0 || y >= Cubes[z].Count) ||
                (x < 0 || x >= Cubes[z][y].Count))
            {
                return false;
            }

            return Cubes[z][y][x];
        }

        private static IEnumerable<Coords3d> GetAllDirections() =>
            Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .Select(z => new Coords3d(x, y, z))));

        public static readonly IReadOnlyCollection<Coords3d> Directions = GetAllDirections()
            .Where(dir => !(dir.X == 0 && dir.Y == 0 && dir.Z == 0))
            .ToArray();

        private IEnumerable<Coords3d> GetActiveNeighbors(Coords3d coords) =>
            Directions
                .Select(dir => new Coords3d(
                    coords.X + dir.X,
                    coords.Y + dir.Y,
                    coords.Z + dir.Z))
                .Where(IsCubeActive);

        public record Coords3d(int X, int Y, int Z)
        {
        }
    }
}
