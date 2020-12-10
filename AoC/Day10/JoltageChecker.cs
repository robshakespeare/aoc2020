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
            var jolts = 0;
            var done = false;
            var arrangements = 1L;
            do
            {
                var oldJolts = jolts;

                var nextPossibleJoltages = GetNextPossibleJoltages(jolts).ToArray();
                var countOfNextPossibleJoltages = nextPossibleJoltages.Length;
                if (countOfNextPossibleJoltages == 0)
                {
                    done = true;
                }
                else
                {
                    var perms = countOfNextPossibleJoltages switch
                    {
                        3 => 4,
                        2 => 2,
                        1 => 1,
                        _ => throw new InvalidOperationException($"Invalid {new { countOfNextPossibleJoltages }}")
                    };
                    arrangements *= perms;
                    jolts = nextPossibleJoltages.Max();
                }

                Console.WriteLine(new { oldJolts, jolts, arrangements, done });
            } while (!done);

            if (jolts != _maxJoltageRating)
            {
                throw new InvalidOperationException($"Reached {jolts} which does not match max jolts of {_maxJoltageRating}");
            }

            Console.WriteLine(new { jolts, arrangements, done });

            return arrangements;
        }

        public static JoltageChecker Parse(string input) => new(input.ReadLines().Select(int.Parse));
    }
}
