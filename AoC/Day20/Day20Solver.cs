using System;
using System.Linq;
using Serilog;

namespace AoC.Day20
{
    public class Day20Solver : SolverBase
    {
        public override string DayName => "Jurassic Jigsaw";

        private static readonly ILogger Logger = FileLogging.CreateLogger("day20-c");

        protected override long? SolvePart1Impl(string input)
        {
            var grid = Grid.ParsePuzzleInput(input);

            Console.WriteLine($"grid.OuterEdges.Count: {grid.OuterEdges.Count}");

            var cornerTiles = grid.Tiles
                .Where(tile => tile.NumOuterEdges == 2)
                .ToArray();

            Console.WriteLine($"cornerTilesCount: {cornerTiles.Length}");
            Console.WriteLine($"cornerTileIds: {string.Join(", ", cornerTiles.Select(x => x.TileId))}");

            return cornerTiles.Aggregate(1L, (agg, tile) => agg * tile.TileId);
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
