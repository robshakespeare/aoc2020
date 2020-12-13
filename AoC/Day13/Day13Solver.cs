using System.Linq;
using MoreLinq;

namespace AoC.Day13
{
    public class Day13Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input)
        {
            var inputLines = input.ReadLines().ToArray();
            var earliestDepartTime = int.Parse(inputLines[0]);
            var earliestBus = inputLines[1].Split(",")
                .Where(x => x != "x")
                .Select(int.Parse)
                .Select(busFrequency =>
                {
                    var intervals = earliestDepartTime / busFrequency + 1;
                    var nextAvailableBusDepartTime = busFrequency * intervals;
                    return new { busId = busFrequency, nextAvailableBusDepartTime };
                })
                .MinBy(x => x.nextAvailableBusDepartTime)
                .First();

            var waitTime = earliestBus.nextAvailableBusDepartTime - earliestDepartTime;

            return earliestBus.busId * waitTime;
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
