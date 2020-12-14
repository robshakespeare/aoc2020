using System.Linq;
using MoreLinq;

namespace AoC.Day13
{
    public class Day13Solver : SolverBase
    {
        public override string DayName => "Shuttle Search";

        protected override long? SolvePart1Impl(string input)
        {
            var inputLines = input.ReadLines().ToArray();
            var earliestDepartTime = int.Parse(inputLines[0]);
            var earliestBus = inputLines[1].Split(",")
                .Where(x => x != "x")
                .Select(int.Parse)
                .Select(busNum => new
                {
                    busNum,
                    nextAvailableBusDepartTime = GetNextAvailableBusDepartTime(busNum, earliestDepartTime)
                })
                .MinBy(x => x.nextAvailableBusDepartTime)
                .First();

            var waitTime = earliestBus.nextAvailableBusDepartTime - earliestDepartTime;

            return earliestBus.busNum * waitTime;
        }

        private static long GetNextAvailableBusDepartTime(int busNum, long earliestDepartTime)
        {
            var busFrequency = busNum; // Note: busFrequency == busNum!
            var intervals = earliestDepartTime / busFrequency + 1;
            var nextAvailableBusDepartTime = busFrequency * intervals;
            return nextAvailableBusDepartTime;
        }

        protected override long? SolvePart2Impl(string input) => GetMatchingDepartureTimesEfficient2(input);

        public static long GetMatchingDepartureTimesEfficient2(string input)
        {
            var inputLine = input.ReadLines().Last();

            var buses = inputLine.Split(",")
                .Select((value, index) => new { value, index })
                .Where(x => x.value != "x")
                .Select(x => new
                {
                    busNum = int.Parse(x.value),
                    offset = x.index
                })
                .ToArray();

            var modsMultiplied = buses.Aggregate(1L, (acc, x) => acc * x.busNum);

            return buses.Select(bus =>
            {
                var coefficient = modsMultiplied / bus.busNum;
                var bigN = SolveBigN(coefficient, bus.busNum, bus.busNum - bus.offset);
                return coefficient * bigN;
            }).Sum() % modsMultiplied;

            static int SolveBigN(long a, int m, int n) => Enumerable.Range(0, m).First(i => Modulus(a * i, m) == Modulus(n, m));
        }

        /// <summary>
        /// Returns the modulus that results from division with two specified integer values. https://stackoverflow.com/a/18106623
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        private static long Modulus(long dividend, long divisor) => (dividend % divisor + divisor) % divisor;

        public static long GetMatchingDepartureTimesBruteForce(string input)
        {
            var inputLine = input.ReadLines().Last();

            var buses = inputLine.Split(",")
                .Select((value, index) => new { value, index })
                .Where(x => x.value != "x")
                .Select(x =>
                {
                    var busNum = int.Parse(x.value);
                    var offset = x.index;
                    return new { busNum, offset };
                })
                .ToArray();

            // Keep enumerating the first buses known times, searching for the first where all bus' next departure delta matches their offset
            var firstBus = buses.First();
            var otherBuses = buses[1..];

            var matchingDepartureTime = Enumerable.Range(1, int.MaxValue)
                .Select(departureNumber => new { departureNumber, departureTime = firstBus.busNum * departureNumber })
                .First(x =>
                {
                    return otherBuses.All(otherBus =>
                    {
                        var nextDepartureTime = GetNextAvailableBusDepartTime(otherBus.busNum, x.departureTime);
                        var delta = nextDepartureTime - x.departureTime;
                        return delta == otherBus.offset;
                    });
                });

            return matchingDepartureTime.departureTime;
        }
    }
}
