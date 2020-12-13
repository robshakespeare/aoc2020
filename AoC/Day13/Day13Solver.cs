using System;
using System.Linq;
using System.Reflection;
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
            var inputLine = input.ReadLines().ToArray().Last();

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

            var timestamps = inputLine.Split(",")
                .Select((value, index) => new {value, index})
                .Where(x => x.value != "x")
                .Select(x =>
                {
                    var busId = int.Parse(x.value);
                    var offset = x.index;
                    var timestamp = busId + offset;
                    var lcm = MathUtils.LeastCommonMultiple(busId, timestamp);
                    return new {busId, offset, timestamp, lcm};
                })
                .ToArray();

            Console.WriteLine(ObjectDumper.Dump(timestamps));

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
