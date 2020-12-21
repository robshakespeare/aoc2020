using System;
using System.Linq;
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

            // Work out the arrangement of the edge of the proper reassembled grid

            // Once have edges, use a search to find the full arrangement of the proper reassembled grid

            // Once have the reassembled grid, look for monsters!

            return null;
        }
    }
}
