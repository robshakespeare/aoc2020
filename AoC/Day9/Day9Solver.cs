namespace AoC.Day9
{
    public class Day9Solver : SolverBase
    {
        public override string DayName => "Encoding Error";

        protected override long? SolvePart1Impl(string input) => XmasCracker.Parse(input, 25).GetFirstInvalidNumber();

        protected override long? SolvePart2Impl(string input) => XmasCracker.Parse(input, 25).GetEncryptionWeakness();
    }
}
