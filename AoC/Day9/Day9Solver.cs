namespace AoC.Day9
{
    public class Day9Solver : SolverBase
    {
        public override string DayName => "Encoding Error";

        protected override long? SolvePart1Impl(string input) => XmasCracker.Parse(input, 25).GetFirstNumAfterPreambleWhichIsNotSumOfTwoOfPreviousBlock();

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
