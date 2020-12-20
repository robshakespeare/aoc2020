using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day20
{
    public class Grid
    {
        public IReadOnlyList<Tile> Tiles { get; private set; } = Array.Empty<Tile>();
        public int GridSize { get; private set; }

        private Grid()
        {
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

            return grid;
        }
    }
}
