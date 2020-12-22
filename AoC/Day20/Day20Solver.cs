using System;
using System.Linq;

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

            grid.ReassembleFullGridBorder();

            grid.LogNumPermsForEachCornerTile();
            grid.LogCornerTilePerms();

            

            // Work out the arrangement of the edge of the proper reassembled grid

            // Once have edges, use a search to find the full arrangement of the proper reassembled grid

            // Once have the reassembled grid, look for monsters!
            // !! IMPORTANT: DON'T FORGET: remove the "border" first, i.e. the outside pixels from EACH TILE of the reassembled grid
            // rs-todo: somewhere at this point, ave it in the method so it can be called, to verify that the example input produces the expected "reassembled grid"

            return null;
        }
    }
}
