using System;
using System.Collections.Generic;
using MoreLinq;
using static System.Environment;

namespace AoC.Day20
{
    using System.Linq;

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
                .Split($"{NewLine}{NewLine}")
                .Select(tileString => Tile.ParseTile(tileString, grid))
                .ToArray();

            grid.GridSize = (int) Math.Sqrt(tiles.Length);
            grid.Tiles = tiles;
            grid.RebuildOuterEdges();
            grid.OuterEdgeCornerTiles = grid.Tiles.Where(tile => tile.IsOuterEdgeCornerTile).ToArray();
            grid.OuterEdgeNonCornerTiles = grid.Tiles.Where(tile => tile.IsOuterEdgeNonCornerTile).ToArray();

            return grid;
        }

        public void LogNumPermsForEachCornerTile()
        {
            foreach (var cornerTile in OuterEdgeCornerTiles)
            {
                Console.WriteLine($"{NewLine}Corner tile {cornerTile.TileId}");

                foreach (var corner in Corner.All)
                {
                    Console.WriteLine($"Num perms for Corner {corner}: {cornerTile.GetPermsForCorner(corner).Count()}");
                }
            }
        }

        public void LogCornerTilePerms()
        {
            Console.WriteLine($"{NewLine}cornerTilePerms:");
            var cornerTilesPerms = OuterEdgeCornerTiles.Permutations().ToArray();

            Console.WriteLine(string.Join(
                NewLine,
                cornerTilesPerms
                    .Select(cornerTilePerm =>
                        new
                        {
                            id = string.Join(", ", cornerTilePerm.Select(tile => tile.TileId)),
                            permsPerCorner = string.Join(", ",
                                cornerTilePerm.Select((tile, cornerIndex) => tile.GetPermsForCorner(Corner.All[cornerIndex]).Count())),
                            totalPerms = cornerTilePerm.Select((tile, cornerIndex) => tile.GetPermsForCorner(Corner.All[cornerIndex]).Count())
                                .Aggregate(1, (agg, cur) => agg * cur)
                        })
                    .OrderBy(x => x.totalPerms)));
        }

        ////public record FullGridBorderState()
        ////{
        ////}

        public void ReassembleFullGridBorder()
        {
            var cornerTilesPerms = OuterEdgeCornerTiles.Permutations().ToArray();

            // rs-todo: resume here????!!!!!

            //// Start in the top right, and try and build out the border
            //// One of the OuterEdgeCornerTiles MUST be in the top right, so just try each
            //foreach (var outerEdgeCornerTile in OuterEdgeCornerTiles)
            //{
            //    foreach (var topRightTilePerm in outerEdgeCornerTile.GetPermsForCorner(Corner.TopRight))
            //    {
            //    }
            //}

            var cornerTilePermsOrdered = cornerTilesPerms
                .Select(cornerTilePerm =>
                    new
                    {
                        cornerTilePerm,
                        totalPerms = cornerTilePerm.Select((tile, cornerIndex) => tile.GetPermsForCorner(Corner.All[cornerIndex]).Count())
                            .Aggregate(1, (agg, cur) => agg * cur)
                    })
                .OrderBy(x => x.totalPerms)
                .Select(x => x.cornerTilePerm);

            // For each permutation of corner tiles, try each permutation of orientations
            // And build out, until we reach a point where's there's no match
            // When there's no match, step back until there is a match, and try that again
            foreach (var cornerTilesPerm in cornerTilePermsOrdered)
            {
                // Put each possible perm of each tile in to its corner

                foreach (var corner in Corner.All)
                {
                    var tile = cornerTilesPerm[corner.CornerIndex];
                }



                // .Select((cornerTilePerm, cornerIndex) => (cornerTilePerm, cornerIndex)

                //cornerTilePerm[cornerIndex]
            }
        }
    }
}
