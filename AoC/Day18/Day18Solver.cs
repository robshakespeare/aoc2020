using System.Linq;

namespace AoC.Day18
{
    public class Day18Solver : SolverBase
    {
        public override string DayName => "Operation Order";

        private static long Solve(string input, ExpressionEvaluator3 expressionEvaluator) =>
            input.ReadLines().Select(expressionEvaluator.Evaluate).Sum();

        protected override long? SolvePart1Impl(string input) => Solve(input, ExpressionEvaluator3.Part1Evaluator);

        protected override long? SolvePart2Impl(string input) => Solve(input, ExpressionEvaluator3.Part2Evaluator);
    }
}
