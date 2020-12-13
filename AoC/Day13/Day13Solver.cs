using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoreLinq;
using Serilog;

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
                .Select(busId => new
                {
                    busId,
                    nextAvailableBusDepartTime = GetNextAvailableBusDepartTime(busId, earliestDepartTime)
                })
                .MinBy(x => x.nextAvailableBusDepartTime)
                .First();

            var waitTime = earliestBus.nextAvailableBusDepartTime - earliestDepartTime;

            return earliestBus.busId * waitTime;
        }

        public static long GetNextAvailableBusDepartTime(int busId, long earliestDepartTime)
        {
            var busFrequency = busId; // Note: busFrequency == busId!
            var intervals = earliestDepartTime / busFrequency + 1;
            var nextAvailableBusDepartTime = busFrequency * intervals;
            return nextAvailableBusDepartTime;
        }

        // rs-todo: is `ExtendedGcd` needed?:
        // https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm#Pseudocode
        // https://math.stackexchange.com/a/3864593 
        public static void ExtendedGcd(long a, long b)
        {
            (long old, long cur) r = (a, b);
            (long old, long cur) s = (1, 0);
            (long old, long cur) t = (0, 1);

            while (r.cur != 0)
            {
                var quotient = r.old / r.cur;
                r = (r.cur, r.old - quotient * r.cur);
                s = (s.cur, s.old - quotient * s.cur);
                t = (t.cur, t.old - quotient * t.cur);
            }

            Console.WriteLine($"Bézout coefficients: ({s.old}, {t.old})");
            Console.WriteLine($"greatest common divisor: {r.old}");
            Console.WriteLine($"quotients by the gcd: ({t.cur}, {s.cur})");
        }

        public static long GetMatchingDepartureTimes(string input, long startSearchingAtTimestamp = 1, ILogger? logger = null)
        {
            var inputLine = input.ReadLines().ToArray().Last();

            var buses = inputLine.Split(",")
                .Select((value, index) => new { value, index })
                .Where(x => x.value != "x")
                .Select(x =>
                {
                    var busId = int.Parse(x.value);
                    var offset = x.index;
                    return new { busId, offset };
                })
                .ToArray();

            // Keep enumerating the first buses known times, searching for the first where all bus' next departure delta matches their offset
            var firstBus = buses.First();
            var otherBuses = buses[1..];
            Console.WriteLine($"firstBus: {firstBus}");

            var startSearchingAtDepartureNumber = startSearchingAtTimestamp / firstBus.busId + 1;

            var stopwatch = Stopwatch.StartNew();
            const int logEverySeconds = 10;
            var logEvery = TimeSpan.FromSeconds(logEverySeconds);
            var lastLogTime = DateTime.UtcNow.AddSeconds(-logEverySeconds * 2);

            var matchingDepartureTime = LongRange(startSearchingAtDepartureNumber)
                .Select(departureNumber => new { departureNumber, departureTime = firstBus.busId * departureNumber })
                .First(x =>
                {
                    if (DateTime.UtcNow - lastLogTime >= logEvery)
                    {
                        logger?.Information($"Elapsed {stopwatch.Elapsed}: {x}");
                        lastLogTime = DateTime.UtcNow;
                    }

                    return otherBuses.All(otherBus =>
                    {
                        var nextDepartureTime = GetNextAvailableBusDepartTime(otherBus.busId, x.departureTime);
                        var delta = nextDepartureTime - x.departureTime;
                        return delta == otherBus.offset;
                    });
                });

            return matchingDepartureTime.departureTime;
        }

        private static IEnumerable<long> LongRange(long start, long end = long.MaxValue)
        {
            for (var current = start; current < end; current++)
            {
                yield return current;
            }
        }

        /// <remarks>
        /// It is the first one that's important, in that the first one must be divisible without remainder by the answer
        /// DEFINITE: So, the answer is always a multiple of the first.
        /// </remarks>
        protected override long? SolvePart2Impl(string input)
        {
            return GetMatchingDepartureTimes(input, 200020301065896, FileLogging.CreateLogger("Day13"));

            //var numBlanks = 0
            //foreach (var part in inputLines[1].Split(","))
            //{
            //    if (part == "x")
            //    {
            //        numBlanks++;
            //    }
            //    else
            //    {

            //        numBlanks = 0;
            //    }
            //}

            //var timestamps = inputLine.Split(",")
            //    .Select((value, index) => new {value, index})
            //    .Where(x => x.value != "x")
            //    .Select(x =>
            //    {
            //        var busId = int.Parse(x.value);
            //        var offset = x.index;
            //        var timestamp = busId + offset;
            //        var lcm = MathUtils.LeastCommonMultiple(busId, timestamp);
            //        return new {busId, offset, timestamp, lcm};
            //    })
            //    .ToArray();

            //Console.WriteLine(ObjectDumper.Dump(timestamps));

            //.Aggregate(
            //    new { prevTimestamp = 0, numBlanks = 0 },
            //    (accumulate, current) =>
            //    {
            //        if (current == "x")
            //        {
            //            return new { accumulate.prevTimestamp, numBlanks = accumulate.numBlanks + 1 };
            //        }

            //        var busId = int.Parse(current);
            //        var timestamp = busId + accumulate.numBlanks;
            //        return new { accumulate.prevTimestamp, numBlanks = accumulate.numBlanks + 1 };
            //    });

            return -1;
        }
    }
}
