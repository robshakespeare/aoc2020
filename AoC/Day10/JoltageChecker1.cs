using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day10
{
    public class JoltageChecker1
    {
        private readonly IReadOnlySet<int> _joltageRatings;
        private readonly int _maxJoltageRating;

        public JoltageChecker1(IEnumerable<int> joltageRatings)
        {
            _joltageRatings = new HashSet<int>(joltageRatings);
            _maxJoltageRating = _joltageRatings.Max();
        }

        private int GetNextPossibleJoltage(int currentJolts) =>
            Enumerable.Range(currentJolts + 1, 3).FirstOrDefault(joltage => _joltageRatings.Contains(joltage));

        public (int numOf1JoltDiffs, int numOf2JoltDiffs, int numOf3JoltDiffs) CountJoltageDifferences()
        {
            var prevJolts = 0;
            int currentJolts;
            int[] joltageDiffs = new int[4];
            while ((currentJolts = GetNextPossibleJoltage(prevJolts)) != 0)
            {
                var diff = currentJolts - prevJolts;

                joltageDiffs[diff]++;
                prevJolts = currentJolts;
            }

            if (prevJolts != _maxJoltageRating)
            {
                throw new InvalidOperationException($"Reached {prevJolts} which does not match max jolts of {_maxJoltageRating}");
            }

            joltageDiffs[3]++;

            return (joltageDiffs[1], joltageDiffs[2], joltageDiffs[3]);
        }

        public long GetPart1Answer()
        {
            var (numOf1JoltDiffs, _, numOf3JoltDiffs) = CountJoltageDifferences();
            return numOf1JoltDiffs * numOf3JoltDiffs;
        }

        public static JoltageChecker1 Parse(string input) => new(input.ReadLines().Select(int.Parse));
    }
}
