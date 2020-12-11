namespace AoC.Day11
{
    public class Day11Solver : SolverBase
    {
        public override string DayName => "Seating System";

        // rs-todo: part 1 is slow!
        protected override long? SolvePart1Impl(string input)
        {
            SeatingGrid newGrid = new(input);
            SeatingGrid oldGrid;
            do
            {
                oldGrid = newGrid;
                newGrid = oldGrid.GenerateNextGrid();
            } while (newGrid.Grid != oldGrid.Grid);

            return newGrid.CountOccupiedSeats();
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
