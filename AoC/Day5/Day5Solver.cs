using System;
using System.Linq;

namespace AoC.Day5
{
    public class Day5Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => input.ReadLines().Select(Seat.Parse).Max(seat => seat.Id);

        protected override long? SolvePart2Impl(string input)
        {
            int? prevSeatId = null;
            foreach (var seat in input.ReadLines().Select(Seat.Parse).OrderBy(seat => seat.Id))
            {
                if (prevSeatId != null && prevSeatId + 1 != seat.Id) return prevSeatId + 1;
                prevSeatId = seat.Id;
            }
            return null;
        }
    }

    public record Seat(int Row, int Column)
    {
        public int Id { get; } = Row * 8 + Column;

        public static Seat Parse(string ticketRef) => new(
            ParseSection(ticketRef[0..7], (0, 127), 'F', 'B'),
            ParseSection(ticketRef[7..10], (0, 7), 'L', 'R'));

        private static int ParseSection(string ticketRefSection, (int start, int end) range, char lower, char upper)
        {
            foreach (var chr in ticketRefSection)
            {
                int mid = range.start + (range.end - range.start) / 2;

                if (chr == lower) range = (range.start, mid);
                else if (chr == upper) range = (mid + 1, range.end);
                else throw new InvalidOperationException($"Unexpected char {chr}");
            }
            return range.start;
        }
    }
}
