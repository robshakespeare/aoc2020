namespace AoC.Day10
{
    public class Day10Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input) => JoltageChecker.Parse(input).GetPart1Answer();

        protected override long? SolvePart2Impl(string input) => JoltageChecker2.CountDistinctArrangements(input);
    }
}
