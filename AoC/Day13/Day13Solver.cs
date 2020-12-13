using System;
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

        private static long GetNextAvailableBusDepartTime(int busId, long earliestDepartTime)
        {
            var busFrequency = busId; // Note: busFrequency == busId!
            var intervals = earliestDepartTime / busFrequency + 1;
            var nextAvailableBusDepartTime = busFrequency * intervals;
            return nextAvailableBusDepartTime;
        }

        // https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm#Pseudocode
        /// <summary>
        /// Extended Greatest Common Divisor Algorithm
        /// oldR == gcd: The greatest common divisor of a and b.
        /// oldS, oldT == Bézout Coefficients such that s*a + t*b = gcd
        /// </summary>
        public static (long oldR, long oldS, long oldT) ExtendedGcd(long a, long b)
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

            // Bézout coefficients: (s.old, t.old)
            // greatest common divisor: r.old
            // quotients by the gcd: (t.cur, s.cur)

            return (r.old, s.old, t.old);
        }

        /*
         *  """Combine two phased rotations into a single phased rotation

            Returns: combined_period, combined_phase

            The combined rotation is at its reference point if and only if both a and b
            are at their reference points.
            """
         */
        public static (long combined_period, long combined_phase) combine_phased_rotations(long a_period, long a_phase, long b_period, long b_phase)
        {
            var (gcd, s, t) = ExtendedGcd(a_period, b_period);

            var phase_difference = a_phase - b_phase;

            //pd_mult, pd_remainder = divmod(phase_difference, gcd);
            var pd_mult = phase_difference / gcd;
            var pd_remainder = mod(phase_difference, gcd);

            //if (pd_remainder == 0)
            //{
            //    throw new InvalidOperationException("Rotation reference points never synchronize.");
            //}

            var combined_period = a_period / gcd * b_period;
            var combined_phase = mod((a_phase - s * pd_mult * a_period), combined_period);
            return (combined_period, combined_phase);
        }

        public static long arrow_alignment(long red_len, long green_len, long advantage)
        {
            // """Where the arrows first align, where green starts shifted by advantage"""
            var (period, phase) = combine_phased_rotations(red_len, 0, green_len, mod(-advantage, green_len));
            return mod(-phase, period);
        }

        private static long mod(long a, long n)
        {
            long result = a % n;
            if ((result < 0 && n > 0) || (result > 0 && n < 0))
            {
                result += n;
            }
            return result;
        }

        public static long GetMatchingDepartureTimesBruteForce(string input)
        {
            var inputLine = input.ReadLines().Last();

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

            var matchingDepartureTime = Enumerable.Range(1, int.MaxValue)
                .Select(departureNumber => new { departureNumber, departureTime = firstBus.busId * departureNumber })
                .First(x =>
                {
                    return otherBuses.All(otherBus =>
                    {
                        var nextDepartureTime = GetNextAvailableBusDepartTime(otherBus.busId, x.departureTime);
                        var delta = nextDepartureTime - x.departureTime;
                        return delta == otherBus.offset;
                    });
                });

            return matchingDepartureTime.departureTime;
        }

        public static long GetMatchingDepartureTimesEfficient(string input)
        {
            var inputLine = input.ReadLines().Last();

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

            //Console.WriteLine(arrow_alignment(7, 13, 1));

            long xyz = firstBus.busId;

            Console.WriteLine($"{xyz} (xyz)");

            foreach (var otherBus in otherBuses)
            {
                xyz = arrow_alignment(xyz, otherBus.busId, otherBus.offset);

                Console.WriteLine(xyz);
            }

            return -1;
        }

        /// <remarks>
        /// It is the first one that's important, in that the first one must be divisible without remainder by the answer
        /// DEFINITE: So, the answer is always a multiple of the first.
        /// </remarks>
        protected override long? SolvePart2Impl(string input)
        {
            return GetMatchingDepartureTimesEfficient(input);

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
