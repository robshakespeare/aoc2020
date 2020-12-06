using System.Linq;
using static System.Environment;

namespace AoC.Day6
{
    public class Day6Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => input
            .Split($"{NewLine}{NewLine}")
            .Select(group => group.Where(c => c >= 'a' && c <= 'z').Distinct())
            .Aggregate(0, (accumulate, current) => accumulate + current.Count());

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
