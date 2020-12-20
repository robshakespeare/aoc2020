using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day20
{
    public class Grid
    {
        public IReadOnlyList<Tile> Tiles { get; private set; } = Array.Empty<Tile>();
        public int GridSize { get; private set; }
        public HashSet<string> OuterEdges { get; private set; } = new();

        private Grid()
        {
        }

        private void RebuildOuterEdges()
        {
            var allEdges = Tiles.SelectMany(tile => tile.GetAllPermsOfEdges());

            var unpairedEdges = allEdges.GroupBy(edge => edge)
                .Where(grp => grp.Count() == 1)
                .Select(grp => grp.Single())
                .ToHashSet();

            OuterEdges = unpairedEdges;
        }

        public static Grid ParsePuzzleInput(string input)
        {
            var grid = new Grid();

            var tiles = input.NormalizeLineEndings()
                .Split($"{Environment.NewLine}{Environment.NewLine}")
                .Select(tileString => Tile.ParseTile(tileString, grid))
                .ToArray();

            grid.GridSize = (int)Math.Sqrt(tiles.Length);
            grid.Tiles = tiles;
            grid.RebuildOuterEdges();

            return grid;
        }
    }
}
