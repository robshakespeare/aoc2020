using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day10
{
    public class JoltageChecker
    {
        private readonly IReadOnlySet<int> _joltageRatings;
        private readonly int _maxJoltageRating;

        public JoltageChecker(IEnumerable<int> joltageRatings)
        {
            _joltageRatings = new HashSet<int>(joltageRatings);
            _maxJoltageRating = _joltageRatings.Max();
        }

        private int GetNextPossibleJoltage(int currentJolts) => GetNextPossibleJoltages(currentJolts).FirstOrDefault();

        private IEnumerable<int> GetNextPossibleJoltages(int currentJolts) =>
            Enumerable.Range(currentJolts + 1, 3).Where(joltage => _joltageRatings.Contains(joltage));

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

        public long CountDistinctArrangements()
        {
            var distinctArrangements = 0L;
            CountDistinctArrangements(0, ref distinctArrangements, "(0)");
            return distinctArrangements;
        }

        private void CountDistinctArrangements(int currentJolts, ref long distinctArrangements, string path)
        {
            if (currentJolts == _maxJoltageRating)
            {
                distinctArrangements++;
                //Console.WriteLine($"distinctArrangements is now {distinctArrangements} - path: {path}");
                return;
            }

            var nextPossibleJoltages = GetNextPossibleJoltages(currentJolts).ToArray();

            if (nextPossibleJoltages.Length == 0 && currentJolts != _maxJoltageRating)
            {
                throw new InvalidOperationException("Invalid path detected: " + path);
            }

            if (nextPossibleJoltages.Length == 0 && currentJolts != _maxJoltageRating)
            {
                Console.WriteLine($"** NOTE: reached no more possible jolts, but did not reach end {new { currentJolts, _maxJoltageRating }}");
            }

            foreach (var nextJoltage in nextPossibleJoltages)
            {
                CountDistinctArrangements(nextJoltage, ref distinctArrangements, path + ", " + nextJoltage);
            }
        }

        public static JoltageChecker Parse(string input) => new(input.ReadLines().Select(int.Parse));
    }
}
