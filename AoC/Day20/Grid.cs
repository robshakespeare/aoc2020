using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day20
{
    public class Grid
    {
        private HashSet<string>? _outerEdges;

        public IReadOnlyList<Tile> Tiles { get; private set; } = Array.Empty<Tile>();

        public int GridSize { get; private set; }

        public HashSet<string> OuterEdges => _outerEdges ?? throw new InvalidOperationException($"{nameof(RebuildOuterEdges)} must be called first");

        public IReadOnlyList<Tile> OuterEdgeCornerTiles { get; private set; } = Array.Empty<Tile>();

        public IReadOnlyList<Tile> OuterEdgeNonCornerTiles { get; private set; } = Array.Empty<Tile>();

        private Grid()
        {
        }

        private void RebuildOuterEdges()
        {
            static IEnumerable<string> GetAllPermsOfEdges(Tile tile) => tile.TileEdgePerms.SelectMany(p => p.Edges.All).Distinct();

            if (_outerEdges != null)
            {
                throw new InvalidOperationException($"{nameof(RebuildOuterEdges)} can only be called once");
            }

            var allEdges = Tiles.SelectMany(GetAllPermsOfEdges);
            var unpairedEdges = allEdges.GroupBy(edge => edge)
                .Where(grp => grp.Count() == 1)
                .Select(grp => grp.Single())
                .ToHashSet();
            _outerEdges = unpairedEdges;
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
            grid.OuterEdgeCornerTiles = grid.Tiles.Where(tile => tile.IsOuterEdgeCornerTile).ToArray();
            grid.OuterEdgeNonCornerTiles = grid.Tiles.Where(tile => tile.IsOuterEdgeNonCornerTile).ToArray();

            return grid;
        }
    }
}
