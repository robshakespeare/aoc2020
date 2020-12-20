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

            // For each tile perm, get all the other tile perms
            // Lay them out in the grid
            // Check all their edges match, break out as soon as they don't
            // Its the first one where all match that we're looking for (assuming there's only one solution!)

            //var tileIds = tiles.Select(x => x.TileId).ToArray();
            //Console.WriteLine(ObjectDumper.Dump(tileIds.Permutations()));
            //return tileIds.Permutations().First().Count(); //tiles.Select(x => x.TileId).Permutations().Count();
            // HMMMMM, this just isn't going to work, there's far too many permutations
            // it is factorial!

            // Idea though, have we really just got a tree, which we can do a BFS on?

            // Or, can we do a process of elimination to reduce the sets?

            var counts = grid.Tiles.SelectMany(tile => tile.GetEdges().GetAll().Select(edge => edge.Count(c => c == '#')))
                    .GroupBy(x => x)
                    .Select(grp => new { numOfPaintedPixels = grp.Key, countOfEdges = grp.Count() });

            Logger.Information($"Group counts:{Environment.NewLine}{string.Join(Environment.NewLine, counts)}");

            // rs-todo: resume here!!

            return null;
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
