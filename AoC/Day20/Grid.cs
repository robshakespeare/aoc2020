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

        private HashSet<string>? _innerEdges;

        public IReadOnlyList<Tile> Tiles { get; private set; } = Array.Empty<Tile>();

        public int GridSize { get; private set; }

        public HashSet<string> OuterEdges => _outerEdges ?? throw new InvalidOperationException($"{nameof(RebuildOuterAndInnerEdges)} must be called first");

        public HashSet<string> InnerEdges => _innerEdges ?? throw new InvalidOperationException($"{nameof(RebuildOuterAndInnerEdges)} must be called first");

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
            _innerEdges = pairedEdges;

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

        public void LogNumPermsForEachCornerTile()
        {
            var displayExtra = true;

            foreach (var cornerTile in OuterEdgeCornerTiles)
            {
                Console.WriteLine($"{NewLine}Corner tile {cornerTile.TileId}");

                foreach (var corner in Corner.All)
                {
                    Console.WriteLine($"Num perms for Corner {corner}: {cornerTile.GetOrientationsForCorner(corner).Count()}");
                    if (displayExtra)
                    {
                        Console.WriteLine($"{string.Join(NewLine + NewLine, cornerTile.GetOrientationsForCorner(corner).Select(x => x.Id))}");
                        displayExtra = false;
                    }
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
                                cornerTilePerm.Select((tile, cornerIndex) => tile.GetOrientationsForCorner(Corner.All[cornerIndex]).Count())),
                            totalPerms = cornerTilePerm.Select((tile, cornerIndex) => tile.GetOrientationsForCorner(Corner.All[cornerIndex]).Count())
                                .Aggregate(1, (agg, cur) => agg * cur)
                        })
                    .OrderBy(x => x.totalPerms)));
        }

        ////public record FullGridBorderState()
        ////{
        ////}

        private static TileOrientation UninitializedTileOrientation { get; } = new(
            new Tile(int.MinValue, new[] {"UninitializedTile"}, new Grid()),
            new Orientation(default, default));

        public void ReassembleFullGrid()
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
                var prev = newGrid[0][x - 1] ?? throw new InvalidOperationException("Prev tile orientation is null");
                newGrid[0][x] = prev.GetUniquePairToRight();
            }

            Console.WriteLine(newGrid[0][^1].Tile.IsOuterEdgeCornerTile);

            if (newGrid[0][^1].Tile.IsOuterEdgeCornerTile != true)
            {
                throw new InvalidOperationException("Build out row failed, we didn't end up with a corner tile in the other corner");
            }


            //var grid = new char[GridSize][];

            //var firstCorner = OuterEdgeCornerTiles.First();

            //var test = firstCorner
            //    .GetOrientationsForCorner(Corner.TopRight)
            //    .Select(tilePerm =>
            //    {
            //        //var search = tilePerm.Edges[TileEdgeLocation.Bottom];

            //        return (tilePerm,
            //            Tiles
            //                .Where(tile => tile != tilePerm.Tile)
            //                .SelectMany(tile => tile.TileOrientations)
            //                .Single(otherTilePerm =>
            //                    tilePerm.Edges[TileEdgeLocation.Bottom] == otherTilePerm.Edges[TileEdgeLocation.Top]));
            //    })
            //    .First();


            //Console.WriteLine($"{test.tilePerm.Id}{NewLine}match?{NewLine}{test.Item2.Id}");

            //Console.WriteLine($"{test.tilePerm.Tile.IsOuterEdgeCornerTile}{NewLine}match?{NewLine}{test.Item2.Tile.IsOuterEdgeNonCornerTile}");





            //var cornerTilesPerms = OuterEdgeCornerTiles.Permutations().ToArray();

            //Console.WriteLine("cornerTilesPerms count: " + cornerTilesPerms.Length);

            //foreach (var (corner, tilePerm) in Corner.All.SelectMany(
            //    corner => OuterEdgeCornerTiles.SelectMany(
            //        cornerTile => cornerTile.TileOrientations.Select(
            //            cornerTilePerm => (corner, cornerTilePerm)))))
            //{

            //}


            ////cornerTilesPerms.Select(tile => tile)

            //// rs-todo: resume here????!!!!!

            ////// Start in the top right, and try and build out the border
            ////// One of the OuterEdgeCornerTiles MUST be in the top right, so just try each
            ////foreach (var outerEdgeCornerTile in OuterEdgeCornerTiles)
            ////{
            ////    foreach (var topRightTilePerm in outerEdgeCornerTile.GetPermsForCorner(Corner.TopRight))
            ////    {
            ////    }
            ////}

            //var cornerTilePermsOrdered = cornerTilesPerms
            //    .Select(cornerTilePerm =>
            //        new
            //        {
            //            cornerTilePerm,
            //            totalPerms = cornerTilePerm.Select((tile, cornerIndex) => tile.GetOrientationsForCorner(Corner.All[cornerIndex]).Count())
            //                .Aggregate(1, (agg, cur) => agg * cur)
            //        })
            //    .OrderBy(x => x.totalPerms)
            //    .Select(x => x.cornerTilePerm);

            //// For each permutation of corner tiles, try each permutation of orientations
            //// And build out, until we reach a point where's there's no match
            //// When there's no match, step back until there is a match, and try that again
            //foreach (var cornerTilesPerm in cornerTilePermsOrdered)
            //{
            //    // Put each possible perm of each tile in to its corner

            //    foreach (var corner in Corner.All)
            //    {
            //        var tile = cornerTilesPerm[corner.CornerIndex];
            //    }



            //    // .Select((cornerTilePerm, cornerIndex) => (cornerTilePerm, cornerIndex)

            //    //cornerTilePerm[cornerIndex]
            //}
        }
    }
}
