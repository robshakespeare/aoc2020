using System;
using System.Collections.Generic;
using System.Text;
using static System.Environment;

namespace AoC.Day20
{
    using System.Linq;

    public class Grid
    {
        private HashSet<string>? _outerEdges;

        public IReadOnlyList<Tile> Tiles { get; private set; } = Array.Empty<Tile>();

        public int GridSize { get; private set; }

        public HashSet<string> OuterEdges => _outerEdges ?? throw new InvalidOperationException($"{nameof(RebuildOuterAndInnerEdges)} must be called first");

        public IReadOnlyList<Tile> OuterEdgeCornerTiles { get; private set; } = Array.Empty<Tile>();

        public IReadOnlyList<Tile> OuterEdgeNonCornerTiles { get; private set; } = Array.Empty<Tile>();

        private Grid()
        {
        }

        private void RebuildOuterAndInnerEdges()
        {
            static IEnumerable<string> GetAllOrientationsOfEdges(Tile tile) => tile.TileOrientations.SelectMany(p => p.Edges.All()).Distinct();

            if (_outerEdges != null)
            {
                throw new InvalidOperationException($"{nameof(RebuildOuterAndInnerEdges)} can only be called once");
            }

            var allEdges = Tiles.SelectMany(GetAllOrientationsOfEdges);
            var edgeGroups = allEdges.GroupBy(edge => edge).ToArray();

            var unpairedEdges = edgeGroups
                .Where(grp => grp.Count() == 1)
                .Select(grp => grp.Single())
                .ToHashSet();
            _outerEdges = unpairedEdges;

            var pairedEdges = edgeGroups
                .Where(grp => grp.Count() == 2)
                .Select(grp => grp.Distinct().Single())
                .ToHashSet();
            if (!pairedEdges.Any())
            {
                throw new InvalidOperationException("No inner edges found (no paired edges)");
            }

            var invalidEdges = edgeGroups.Where(grp => grp.Count() is not 1 and not 2).ToArray();
            if (invalidEdges.Any())
            {
                throw new InvalidOperationException(
                    "Invalid edges detected (they're neither paired nor unpaired): " + string.Join(", ", invalidEdges.Select(x => x.Key)));
            }
        }

        public static Grid ParsePuzzleInput(string input)
        {
            var grid = new Grid();

            var tiles = input.NormalizeLineEndings()
                .Split($"{NewLine}{NewLine}")
                .Select(tileString => Tile.ParseTile(tileString, grid))
                .ToArray();

            grid.GridSize = (int) Math.Sqrt(tiles.Length);
            grid.Tiles = tiles;
            grid.RebuildOuterAndInnerEdges();
            grid.OuterEdgeCornerTiles = grid.Tiles.Where(tile => tile.IsOuterEdgeCornerTile).ToArray();
            grid.OuterEdgeNonCornerTiles = grid.Tiles.Where(tile => tile.IsOuterEdgeNonCornerTile).ToArray();

            return grid;
        }

        private static TileOrientation UninitializedTileOrientation { get; } = new(
            new Tile(int.MinValue, new[] {"UninitializedTile"}, new Grid()),
            new Orientation(default, default));

        public ReassembledGrid ReassembleFullGrid()
        {
            TileOrientation[][] newGrid = Enumerable.Range(0, GridSize)
                .Select(_ => Enumerable.Repeat(UninitializedTileOrientation, GridSize).ToArray()).ToArray();

            // Place the first orientation of the first tile in the top left
            // It doesn't matter which one, because we know we only have individual paired inner edges, there we always be a solution
            // Remember: for our grid, dimension 1 is Y-axis (down), dimension 2 is X-axis (across)
            newGrid[0][0] = OuterEdgeCornerTiles.First().GetOrientationsForCorner(Corner.TopLeft).First();

            // Build out the top row
            for (var x = 1; x < GridSize; x++)
            {
                var prev = newGrid[0][x - 1];
                newGrid[0][x] = prev.GetUniquePairToRight();
            }

            if (newGrid[0][^1].Tile.IsOuterEdgeCornerTile != true)
            {
                throw new InvalidOperationException("Build out row failed, we didn't end up with a corner tile in the other corner");
            }

            // Build out the columns
            for (var x = 0; x < GridSize; x++)
            {
                for (var y = 1; y < GridSize; y++)
                {
                    var prev = newGrid[y - 1][x];
                    newGrid[y][x] = prev.GetUniquePairToBelow();
                }
            }

            if (newGrid[^1][^1].Tile.IsOuterEdgeCornerTile != true)
            {
                throw new InvalidOperationException("Build out row failed, we didn't end up with a corner tile in the bottom right");
            }

            if (newGrid[^1][0].Tile.IsOuterEdgeCornerTile != true)
            {
                throw new InvalidOperationException("Build out row failed, we didn't end up with a corner tile in the bottom left");
            }

            // Validate all of our Grid IDs are unique
            if (!newGrid.SelectMany(line => line).All(x => x.Tile.TileId > 0))
            {
                throw new InvalidOperationException("Not all TileIds are set");
            }

            var distinctTileIds = newGrid.SelectMany(line => line).Select(x => x.Tile.TileId).Distinct().Count();
            if (distinctTileIds != Tiles.Count)
            {
                throw new InvalidOperationException("Not all TileIds are unique");
            }

            // Remove the "border" first, i.e. the outside pixels from EACH TILE of the reassembled grid
            // Then return the reassembled grid
            var reassembledGrid = new StringBuilder();
            foreach (var newGridRow in newGrid)
            {
                foreach (var fullLine in newGridRow
                    .SelectMany((tileOrientation, xOrder) => tileOrientation.GetPixelsWithoutBorder().Select((line, yOrder) => (line, xOrder, yOrder)))
                    .GroupBy(line => line.yOrder)
                    .OrderBy(grp => grp.Key)
                    .Select(grp => string.Join("", grp.OrderBy(line => line.xOrder).Select(line => line.line))))
                {
                    reassembledGrid.AppendLine(fullLine);
                }
            }

            return new ReassembledGrid(reassembledGrid.ToString().TrimEnd());
        }
    }
}
