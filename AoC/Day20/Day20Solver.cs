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

            // Work out the arrangement to form the proper reassembled grid
            var reassembledGrid = grid.ReassembleFullGrid();

            Console.WriteLine($"Reassembled Grid:{NewLine}{reassembledGrid}{NewLine}");

            // Now we have the reassembled grid, look for marks that aren't sea monsters!
            return reassembledGrid.CountMarksThatAreNotSeaMonsters();
        }
    }
}
