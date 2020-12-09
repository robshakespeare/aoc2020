using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day9
{
    public class Day9Solver : SolverBase
    {
        public override string DayName => "Encoding Error";

        protected override long? SolvePart1Impl(string input)
        {
            return base.SolvePart1Impl(input);
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }

    /// <summary>
    /// eXchange-Masking Addition System (XMAS) encryption cracker.
    /// </summary>
    public class XmasCracker
    {
        private readonly IEnumerable<long> _numbers;
        private readonly int _preambleSize;

        public XmasCracker(IEnumerable<long> numbers, int preambleSize)
        {
            _numbers = numbers;
            _preambleSize = preambleSize;
        }

        public long GetWeakness()
        {
            var previousNumbers = new HashSet<long>();
            var previousSums = new HashSet<long>();

            foreach (var (num, index) in _numbers.Select((num, index) => (num, index)))
            {
                var isSameAsLast = previousNumbers.Any() && previousNumbers.Last() == num;
                if (isSameAsLast)
                {
                    throw new InvalidOperationException($"Duplicate num {num} detected at index {index}");
                }

                var isValid = index < _preambleSize || previousSums.Contains(num);
                if (!isValid)
                {
                    return num;
                }

                // Update previous sums
                foreach (var previousNumber in previousNumbers)
                {
                    previousSums.Add(previousNumber + num);
                }

                // Update previous numbers
                previousNumbers.Add(num);
            }

            throw new InvalidOperationException("Failed to get weakness");
        }

        public static XmasCracker Parse(string input, int preambleSize) => new(input.ReadLinesAsLongs(), preambleSize);
    }
}
