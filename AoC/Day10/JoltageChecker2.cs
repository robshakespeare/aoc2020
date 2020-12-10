using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AoC.Day10
{
    public class JoltageChecker2
    {
        public static long CountDistinctArrangements(string input)
        {
            var prev = 0;
            return input.ReadLines().Select(int.Parse).OrderBy(x => x)
                .Select(jolts =>
                {
                    var delta = jolts - prev;
                    prev = jolts;
                    return new { delta };
                })
                .GroupAdjacent(x => x.delta)
                .Where(x => x.Key == 1)
                .Select(x => CountDistinctArrangementsInGroup(x.Count()))
                .Aggregate(1L, (accumulate, current) => accumulate * current);
        }

        public static long CountDistinctArrangementsInGroup(int groupLength)
        {
            var distinctArrangements = 0L;
            var group = Enumerable.Range(1, groupLength).ToArray();
            CountDistinctArrangementsInGroup(0, groupLength, group, ref distinctArrangements);
            return distinctArrangements;
        }

        private static void CountDistinctArrangementsInGroup(int currentJolts, int groupLength, int[] group, ref long distinctArrangements)
        {
            if (currentJolts == groupLength)
            {
                distinctArrangements++;
            }

            foreach (var nextJoltage in GetNextPossibleJoltages(currentJolts, group))
            {
                CountDistinctArrangementsInGroup(nextJoltage, groupLength, group, ref distinctArrangements);
            }
        }

        private static IEnumerable<int> GetNextPossibleJoltages(int currentJolts, int[] group) =>
            Enumerable.Range(currentJolts + 1, 3).Where(group.Contains);
    }
}
