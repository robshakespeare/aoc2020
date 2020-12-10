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

        // rs-todo: don't think this is needed:
        private int[] GetNextPossibleValidJoltages(int currentJolts)
        {
            var nextPossibleJoltages = GetNextPossibleJoltages(currentJolts);

            return nextPossibleJoltages
                .Where(nextPossibleJoltage => nextPossibleJoltage == _maxJoltageRating || GetNextPossibleJoltages(nextPossibleJoltage).Any())
                .ToArray();
        }

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
            var (numOf1JoltDiffs, numOf2JoltDiffs, numOf3JoltDiffs) = CountJoltageDifferences();

            Console.WriteLine(new { _maxJoltageRating, numOf1JoltDiffs, numOf2JoltDiffs, numOf3JoltDiffs });

            return numOf1JoltDiffs * numOf3JoltDiffs;
        }

        public long CountDistinctArrangementsBruteForce()
        {
            //Console.WriteLine("================");
            //DebugPath();
            var distinctArrangements = 0L;
            CountDistinctArrangementsBruteForce(0, ref distinctArrangements, new[] {0});
            //Console.WriteLine("BRUTE FORCE: total distinctArrangements is " + distinctArrangements);
            Console.WriteLine(" = " + distinctArrangements);
            //Console.WriteLine("----------------");
            return distinctArrangements;
        }

        private void DebugPath()
        {
            var path = _joltageRatings.Concat(new[] { 0, _maxJoltageRating + 3 }).OrderBy(x => x).ToArray();

            int? prev = null;
            int[] diffs = new int[4];
            foreach (var i in path)
            {
                if (prev != null)
                {
                    var diff = i - prev.Value;
                    diffs[diff]++;
                }

                prev = i;
            }

            var diffsObj = new { numOf1JoltDiffs = diffs[1], numOf2JoltDiffs = diffs[2], numOf3JoltDiffs = diffs[3] };
            Console.WriteLine($"full path: {string.Join(", ", path)} {diffsObj}");
        }

        private void CountDistinctArrangementsBruteForce(int currentJolts, ref long distinctArrangements, int[] path)
        {
            if (currentJolts == _maxJoltageRating)
            {
                distinctArrangements++;
                //path = path.Concat(new[] {_maxJoltageRating + 3}).ToArray();

                int? prev = null;
                int[] diffs = new int[4];
                foreach (var i in path)
                {
                    if (prev != null)
                    {
                        var diff = i - prev.Value;
                        diffs[diff]++;
                    }

                    prev = i;
                }

                var doExtra = false;
                var diffsObj = doExtra ? new { numOf1JoltDiffs = diffs[1], numOf2JoltDiffs = diffs[2], numOf3JoltDiffs = diffs[3]} : null;
                var extra = doExtra ? new {_maxJoltageRating, reqRating = _maxJoltageRating + 3} : null;
                Console.WriteLine($"distinctArrangements is now {distinctArrangements} - path: {string.Join(", ", path.Skip(1))} {diffsObj} {extra}");
                return;
            }

            var nextPossibleJoltages = GetNextPossibleJoltages(currentJolts).ToArray();

            if (nextPossibleJoltages.Length == 0 && currentJolts != _maxJoltageRating)
            {
                throw new InvalidOperationException("Invalid path detected: " + string.Join(", ", path));
            }

            if (nextPossibleJoltages.Length == 0 && currentJolts != _maxJoltageRating)
            {
                Console.WriteLine($"** NOTE: reached no more possible jolts, but did not reach end {new { currentJolts, _maxJoltageRating }}");
            }

            foreach (var nextJoltage in nextPossibleJoltages)
            {
                CountDistinctArrangementsBruteForce(nextJoltage, ref distinctArrangements, path.Concat(new[] {nextJoltage}).ToArray());
            }
        }

        public long CountDistinctArrangements()
        {
            Console.WriteLine("================");

            var jolts = 0;
            var done = false;
            var arrangements = 1L;
            do
            {
                var oldJolts = jolts;

                var nextPossibleJoltages = GetNextPossibleValidJoltages(jolts);
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
            Console.WriteLine("OPTIMAL: total distinctArrangements is " + arrangements);
            Console.WriteLine("----------------");
            return arrangements;
        }

        public static JoltageChecker Parse(string input) => new(input.ReadLines().Select(int.Parse));

        public static JoltageChecker ParseCsv(string input) => new(input.Trim().Split(", ").Select(int.Parse));
    }
}
