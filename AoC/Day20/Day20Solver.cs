using System;
using System.Linq;
using MoreLinq;

namespace AoC.Day20
{
    public class Day20Solver : SolverBase
    {
        public override string DayName => "Jurassic Jigsaw";

        protected override long? SolvePart1Impl(string input)
        {
            var (tiles, gridSize) = Tile.ParsePuzzleInput(input);

            // For each tile perm, get all the other tile perms
            // Lay them out in the grid
            // Check all their edges match, break out as soon as they don't
            // Its the first one where all match that we're looking for (assuming there's only one solution!)

            //var tileIds = tiles.Select(x => x.TileId).ToArray();
            //Console.WriteLine(ObjectDumper.Dump(tileIds.Permutations()));
            //return tileIds.Permutations().First().Count(); //tiles.Select(x => x.TileId).Permutations().Count();
            // HMMMMM, this just isn't going to work, there's far too many permutations
            // it is factorial!

            return null;
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
