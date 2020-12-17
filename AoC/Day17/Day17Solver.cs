namespace AoC.Day17
{
    public class Day17Solver : SolverBase
    {
        public override string DayName => "Conway Cubes";

        protected override long? SolvePart1Impl(string input) =>
            PocketDimension3d.Run(new PocketDimension3d(input), 6).CountActiveCubes();

        protected override long? SolvePart2Impl(string input) =>
            PocketDimension4d.Run(new PocketDimension4d(input), 6).ActiveCubes.Count;
    }
}
