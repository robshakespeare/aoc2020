using System;
using System.Linq;
using MoreLinq;
using static System.Environment;

namespace AoC.Day20
{
    public class Day20Solver : SolverBase
    {
        public override string DayName => "Jurassic Jigsaw";

        protected override long? SolvePart1Impl(string input)
        {
            var grid = Grid.ParsePuzzleInput(input);

            Console.WriteLine($"grid.OuterEdges.Count: {grid.OuterEdges.Count}");

            var cornerTiles = grid.OuterEdgeCornerTiles;

            Console.WriteLine($"cornerTilesCount: {cornerTiles.Count}");
            Console.WriteLine($"cornerTileIds: {string.Join(", ", cornerTiles.Select(x => x.TileId))}");

            return cornerTiles.Aggregate(1L, (agg, tile) => agg * tile.TileId);
        }

        protected override long? SolvePart2Impl(string input)
        {
            var grid = Grid.ParsePuzzleInput(input);

            foreach (var cornerTile in grid.OuterEdgeCornerTiles)
            {
                Console.WriteLine($"{NewLine}Corner tile {cornerTile.TileId}");

                foreach (var corner in Corner.All)
                {
                    Console.WriteLine($"Num perms for Corner {corner}: {cornerTile.GetPermsForCorner(corner).Count()}");
                }
            }

            Console.WriteLine($"{NewLine}cornerTilePerms:");
            var cornerTilePerms = grid.OuterEdgeCornerTiles.Permutations().ToArray();

            Console.WriteLine(string.Join(
                NewLine,
                cornerTilePerms
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

            ////foreach (var cornerTilePerm in cornerTilePerms)
            ////{
            ////    var id = string.Join(", ", cornerTilePerm.Select(tile => tile.TileId));

            ////    var perms = cornerTilePerm.Select((tile, cornerIndex) => tile.GetPermsForCorner(Corner.All[cornerIndex]).Count())
            ////        .Aggregate(1, (agg, cur) => agg * cur);

            ////    Console.WriteLine($"{id}: {perms}");
            ////}


            ////cornerTileIndexPerms.ForEach(
            ////    cornerTileIndexes =>
            ////    {
            ////        var tileIds = string.Join(", ", cornerTileIndexes.Select(i => grid.OuterEdgeCornerTiles[i].TileId));
            ////        var totalPerms = 
            ////        Console.WriteLine(
            ////            $"{tileIds}: ");
            ////    });

            // Work out the arrangement of the edge of the proper reassembled grid

            // Once have edges, use a search to find the full arrangement of the proper reassembled grid

            // Once have the reassembled grid, look for monsters!
            // !! IMPORTANT: DON'T FORGET: remove the "border" first, i.e. the outside pixels from each edge of the reassembled grid
            // rs-todo: somewhere at this point, ave it in the method so it can be called, to verify that the example input produces the expected "reassembled grid"

            return null;
        }
    }
}
