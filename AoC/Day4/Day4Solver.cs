using System.Linq;

namespace AoC.Day4
{
    public class Day4Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => PassportBatchParser.ParseBatch(input).Count(passport => passport.IsValid);

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
