using AoC.BootCode;

namespace AoC.Day8
{
    public class Day8Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => BootCodeComputer.Parse(input).Evaluate();

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
