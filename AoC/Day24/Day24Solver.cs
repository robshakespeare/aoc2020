namespace AoC.Day24
{
    public class Day24Solver : SolverBase
    {
        public override string DayName => "Lobby Layout";

        protected override long? SolvePart1Impl(string input) => LobbyLayout.ParsePuzzleInput(input).NumOfTilesBlackSideUp;

        protected override long? SolvePart2Impl(string input) => LobbyLayout.SimulateLivingArtExhibit(input, 100).NumOfTilesBlackSideUp;
    }
}
