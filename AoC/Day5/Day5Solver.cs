using System;
using System.Linq;

namespace AoC.Day5
{
    public class Day5Solver : SolverBase
    {
        public override string DayName => "Binary Boarding";

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
            ParseSection(ticketRef[0..7], (0, 127)),
            ParseSection(ticketRef[7..10], (0, 7)));

        private static int ParseSection(string ticketRefSection, (int start, int end) range)
        {
            foreach (var chr in ticketRefSection)
            {
                int mid = range.start + (range.end - range.start) / 2;
                range = chr switch
                {
                    'F' or 'L' => (range.start, mid),
                    'B' or 'R' => (mid + 1, range.end),
                    _ => throw new InvalidOperationException($"Unexpected char {chr}")
                };
            }
            return range.start;
        }
    }
}
