namespace AoC.Day12
{
    public class Day12Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input) => new Ship().Navigate(input);

        protected override long? SolvePart2Impl(string input) => new Ship2().Navigate(input);
    }
}
