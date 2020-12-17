namespace AoC.Day17
{
    public class Day17Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input) =>
            PocketDimension3d.Run(new PocketDimension3d(input), 6).ActiveCubes.Count;

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
