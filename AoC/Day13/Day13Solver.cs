using System.Linq;
using System.Numerics;
using MoreLinq;

namespace AoC.Day13
{
    public class Day13Solver : SolverBase
    {
        public override string DayName => "Shuttle Search";

        /// <summary>
        /// Returns the the ID of the earliest bus you can take to the airport multiplied by the number of minutes you'll need to wait for that bus.
        /// </summary>
        protected override long? SolvePart1Impl(string input)
        {
            var inputLines = input.ReadLines().ToArray();
            var earliestDepartTime = int.Parse(inputLines[0]);
            var earliestBus = inputLines[1].Split(",")
                .Where(x => x != "x")
                .Select(int.Parse)
                .Select(busNum =>
                {
                    var intervals = earliestDepartTime / busNum + 1; // Note: busNum == busFrequency
                    var nextAvailableBusDepartTime = (long) busNum * intervals;
                    return new {busNum, nextAvailableBusDepartTime};
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

        /// <summary>
        /// Returns the earliest timestamp such that all of the listed bus IDs depart at offsets matching their positions in the list.
        /// </summary>
        protected override long? SolvePart2Impl(string input)
        {
            var inputLine = input.ReadLines().Last();

            var buses = inputLine.Split(",")
                .Select((value, index) => new {value, index})
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
        }

        /// <summary>
        /// Emulate ModInverse, using ModPow, if m is a prime. https://stackoverflow.com/a/15768873
        /// i.e. modinv(a,m) == pow(a,m-2,m) for prime m
        /// </summary>
        private static long SolveBigN(long a, int m, int n) => (long) BigInteger.ModPow(a, m - 2, m) * n;
    }
}
