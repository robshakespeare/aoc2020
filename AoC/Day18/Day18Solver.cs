using System.Linq;

namespace AoC.Day18
{
    public class Day18Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input)
        {
            var expressionEvaluator = new ExpressionEvaluator();
            return input.ReadLines()
                .Select(line => expressionEvaluator.Evaluate(line))
                .Sum();
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
