namespace AoC.Day23
{
    public class Day23Solver : SolverBase
    {
        public override string DayName => "Crab Cups";

        protected override long? SolvePart1Impl(string input)
        {
            var crabCupsGame = new CrabCupsGame(input, false);
            crabCupsGame.Play(100);
            return crabCupsGame.GetCupOrder();
        }

        protected override long? SolvePart2Impl(string input)
        {
            var crabCupsGame = new CrabCupsGame(input, true);
            crabCupsGame.Play(10000000);
            return crabCupsGame.GetPart2Result();
        }
    }
}
