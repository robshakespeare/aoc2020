using System.Linq;

namespace AoC.Day16
{
    public class Day16Solver : SolverBase
    {
        public override string DayName => "Ticket Translation";

        protected override long? SolvePart1Impl(string input) => TrainServiceNotes.Parse(input).GetInvalidTicketValues().Sum();

        protected override long? SolvePart2Impl(string input) => TrainServiceNotes.Parse(input).DetermineOrderOfFields();
    }
}
