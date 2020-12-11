using System;

namespace AoC.Day11
{
    public class Day11Solver : SolverBase
    {
        public override string DayName => "Seating System";

        protected override long? SolvePart1Impl(string input) => RunAndCountOccupiedSeats(input, 4, false);

        protected override long? SolvePart2Impl(string input)
        {
            Console.WriteLine("Day 11 Part 1 currently takes a long time (~9minutes!), press Y to continue:");
            if (Console.ReadKey().KeyChar != 'y')
            {
                return -1;
            }

            return RunAndCountOccupiedSeats(input, 5, true);
        }

        private static long RunAndCountOccupiedSeats(string input, int occupancyThreshold, bool visibilityOccupancy)
        {
            SeatingGrid newGrid = new(input, occupancyThreshold, visibilityOccupancy);
            SeatingGrid oldGrid;
            do
            {
                oldGrid = newGrid;
                newGrid = oldGrid.GenerateNextGrid();
            } while (newGrid.Grid != oldGrid.Grid);

            return newGrid.CountOccupiedSeats();
        }
    }
}
