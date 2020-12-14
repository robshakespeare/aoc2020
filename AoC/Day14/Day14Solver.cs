using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day14
{
    public class Day14Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input)
        {
            Mask? mask = null;
            var memory = new Dictionary<long, long>();

            foreach (Match match in ParseInputRegex.Matches(input))
            {
                if (match.Groups["mask"].Success)
                {
                    mask = new Mask(match.Groups["mask"].Value);
                }
                else
                {
                    var address = long.Parse(match.Groups["address"].Value);
                    var value = long.Parse(match.Groups["value"].Value);
                    memory[address] = (mask ?? throw new InvalidOperationException("No mask")).ApplyTo(value);
                }
            }

            return memory.Sum(x => x.Value);
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }

        private static readonly Regex ParseInputRegex = new(
            @"(mask = (?<mask>[X10]+))|(mem\[(?<address>\d+)\] = (?<value>\d+))",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public class Mask
        {
            private readonly (char c, int i)[] _mask;

            public Mask(string mask)
            {
                _mask = mask.Reverse().Select((c, i) => (c, i)).Where(m => m.c != 'X').ToArray();
            }

            public long ApplyTo(long input)
            {
                var bytes = BitConverter.GetBytes(input);
                var bits = new BitArray(bytes);

                foreach (var (c, i) in _mask)
                {
                    bits[i] = c == '1';
                }

                bits.CopyTo(bytes, 0);
                return BitConverter.ToInt64(bytes);
            }
        }
    }
}
