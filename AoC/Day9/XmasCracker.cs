using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day9
{
    /// <summary>
    /// eXchange-Masking Addition System (XMAS) encryption cracker.
    /// </summary>
    public class XmasCracker
    {
        private readonly long[] _numbers;
        private readonly int _preambleSize;

        public XmasCracker(IEnumerable<long> numbers, int preambleSize)
        {
            _numbers = numbers.ToArray();
            _preambleSize = preambleSize;
        }

        public static XmasCracker Parse(string input, int preambleSize) => new(input.ReadLinesAsLongs(), preambleSize);

        private bool IsSumOfTwoOfPreviousBlockOfNumbers(int currentIndex, long currentNumber)
        {
            var lastBlockOfNumbers = _numbers[(currentIndex - _preambleSize)..currentIndex];
            return lastBlockOfNumbers
                .SelectMany((a, i) => lastBlockOfNumbers
                    .Select((b, j) => (a, i, b, j))
                    .Where(x => x.i != x.j))
                .Any(x => x.a + x.b == currentNumber);
        }

        /// <summary>
        /// Returns the first number in the list (after the preamble) which is not the sum of two of the preamble block size of numbers before it.
        /// </summary>
        public long GetFirstInvalidNumber() =>
            _numbers.Select((num, index) => (num, index))
                .Skip(_preambleSize)
                .First(x => !IsSumOfTwoOfPreviousBlockOfNumbers(x.index, x.num)).num;

        /// <summary>
        /// Finds the encryption weakness, by adding together the smallest and largest number in the first
        /// contiguous set of at least two numbers which sum to the invalid number, found in part 1.
        /// </summary>
        public long GetEncryptionWeakness()
        {
            var invalidNum = GetFirstInvalidNumber();

            var contiguousSets = new List<ContiguousSet>();
            foreach (var (num, index) in _numbers.Select((num, index) => (num, index)).Skip(1))
            {
                var prev = _numbers[index - 1];

                // Update any existing contiguous sets, if any match, then return
                foreach (var contiguousSet in contiguousSets)
                {
                    contiguousSet.Update(num);
                    if (contiguousSet.Total == invalidNum)
                    {
                        return contiguousSet.RangeSize;
                    }
                }

                // Add the new contiguous set, if that matches, then return
                var newContiguousSet = new ContiguousSet(prev, num);
                if (newContiguousSet.Total == invalidNum)
                {
                    return newContiguousSet.RangeSize;
                }
                contiguousSets.Add(newContiguousSet);
            }

            throw new InvalidOperationException($"Failed to {nameof(GetEncryptionWeakness)}");
        }
    }
}
