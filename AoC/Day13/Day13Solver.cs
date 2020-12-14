using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        /// <summary>
        /// Emulate ModInverse, using ModPow, if n is a prime. https://stackoverflow.com/a/15768873
        /// i.e. modinv(x,n) == pow(x,n-2,n) for prime n
        /// </summary>
        public static BigInteger ModInverse(BigInteger x, BigInteger n) => BigInteger.ModPow(x, n - 2, n);

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

            var (pd_mult, pd_remainder) = divmod(phase_difference, gcd);
            //var pd_mult = phase_difference / gcd;
            //var pd_remainder = modulus(phase_difference, gcd);

            if (pd_remainder == 0)
            {
                throw new InvalidOperationException("Rotation reference points never synchronize.");
            }

            var combined_period = a_period / gcd * b_period;
            var combined_phase = modulus((a_phase - s * pd_mult * a_period), combined_period);
            return (combined_period, combined_phase);
        }

        public static long arrow_alignment(long red_len, long green_len, long advantage)
        {
            // """Where the arrows first align, where green starts shifted by advantage"""
            var (period, phase) = combine_phased_rotations(red_len, 0, green_len, modulus(-advantage, green_len));
            return modulus(-phase, period);
        }

        /// <summary>
        /// Extended Greatest Common Divisor Algorithm
        /// https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm#Pseudocode
        /// gcd: The greatest common divisor of a and b.
        /// s, t: Bézout Coefficients such that s*a + t*b = gcd
        /// </summary>
        public static (long gcd, long s, long t) ExtendedGcd(long a, long b)
        {
            (long old_r, long r) r = (a, b);
            (long old_s, long s) s = (1, 0);
            (long old_t, long t) t = (0, 1);

            while (r.r != 0)
            {
                var (quotient, remainder) = divmod(r.old_r, r.r);
                r = (r.r, remainder);
                s = (s.s, s.old_s - quotient * s.s);
                t = (t.t, t.old_t - quotient * t.t);
            }

            return (r.old_r, s.old_s, t.old_t);

            //(long old, long cur) r = (a, b);
            //(long old, long cur) s = (1, 0);
            //(long old, long cur) t = (0, 1);

            //while (r.cur != 0)
            //{
            //    var quotient = r.old / r.cur;
            //    r = (r.cur, r.old - quotient * r.cur);
            //    s = (s.cur, s.old - quotient * s.cur);
            //    t = (t.cur, t.old - quotient * t.cur);
            //}

            //// Bézout coefficients: (s.old, t.old)
            //// greatest common divisor: r.old
            //// quotients by the gcd: (t.cur, s.cur)

            //return (r.old, s.old, t.old);
        }

        //private static long mod(long a, long n)
        //{
        //    long result = a % n;
        //    if ((result < 0 && n > 0) || (result > 0 && n < 0))
        //    {
        //        result += n;
        //    }
        //    return result;
        //}

        private static (long quotient, long remainder) divmod(long dividend, long divisor) => (dividend / divisor, modulus(dividend, divisor));

        /// <summary>
        /// Returns the modulus that results from division with two specified integer values. https://stackoverflow.com/a/18106623
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        private static long modulus(long dividend, long divisor) => (dividend % divisor + divisor) % divisor;

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

        public static long GetMatchingDepartureTimesEfficient(string input)
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

            //Console.WriteLine(arrow_alignment(7, 13, 1));

            long xyz = firstBus.busNum;

            Console.WriteLine($"{xyz} (xyz)");

            foreach (var otherBus in otherBuses)
            {
                xyz = arrow_alignment(xyz, otherBus.busNum, otherBus.offset);

                Console.WriteLine(xyz);
            }

            return -1;
        }

        public static long SolveBigN(long a, long m, long n) => LongRange(0, m).First(i => modulus((a * i), m) == modulus(n, m));

        private static IEnumerable<long> LongRange(long start, long count)
        {
            var end = start + count;
            var current = start;
            while (current < end)
            {
                yield return current;
                current++;
            }
        }

        public static long GetMatchingDepartureTimesEfficient2(string input)
        {
            var inputLine = input.ReadLines().Last();

            var buses = inputLine.Split(",")
                .Select((value, index) => new { value, index })
                .Where(x => x.value != "x")
                .Select(x =>
                {
                    var busNum = long.Parse(x.value);
                    var offset = x.index;
                    return new { busNum, offset };
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

        /// <remarks>
        /// It is the first one that's important, in that the first one must be divisible without remainder by the answer
        /// DEFINITE: So, the answer is always a multiple of the first.
        /// </remarks>
        protected override long? SolvePart2Impl(string input)
        {
            return GetMatchingDepartureTimesEfficient2(input);


            //return GetMatchingDepartureTimesEfficient(input);

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
            //        var busNum = int.Parse(x.value);
            //        var offset = x.index;
            //        var timestamp = busNum + offset;
            //        var lcm = MathUtils.LeastCommonMultiple(busNum, timestamp);
            //        return new {busNum, offset, timestamp, lcm};
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

            //        var busNum = int.Parse(current);
            //        var timestamp = busNum + accumulate.numBlanks;
            //        return new { accumulate.prevTimestamp, numBlanks = accumulate.numBlanks + 1 };
            //    });

            return -1;
        }
    }
}
