namespace AoC.Day23
{
    public class Day23Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input)
        {
            var crabCupsGame = new CrabCupsGame(input);
            crabCupsGame.Play(100);
            return crabCupsGame.GetCupOrder();
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
