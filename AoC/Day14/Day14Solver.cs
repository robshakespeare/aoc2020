using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day14
{
    public class Day14Solver : SolverBase
    {
        public override string DayName => "Docking Data";

        private static readonly Regex ParseInputRegex = new(
            @"(mask = (?<mask>[X10]+))|(mem\[(?<address>\d+)\] = (?<value>\d+))",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Execute the initialization program. What is the sum of all values left in memory after it completes?
        /// </summary>
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

        public class Mask
        {
            private readonly (char c, int i)[] _mask;

            public Mask(string mask) => _mask = mask.Reverse().Select((c, i) => (c, i)).Where(m => m.c != 'X').ToArray();

            public long ApplyTo(long value)
            {
                var bytes = BitConverter.GetBytes(value);
                var bits = new BitArray(bytes);

                foreach (var (c, i) in _mask)
                {
                    bits[i] = c == '1';
                }

                bits.CopyTo(bytes, 0);
                return BitConverter.ToInt64(bytes);
            }
        }


        /// <summary>
        /// Execute the initialization program using an emulator for a version 2 decoder chip. What is the sum of all values left in memory after it completes?
        /// </summary>
        protected override long? SolvePart2Impl(string input)
        {
            MaskV2? mask = null;
            var memory = new Dictionary<long, long>();

            foreach (Match match in ParseInputRegex.Matches(input))
            {
                if (match.Groups["mask"].Success)
                {
                    mask = new MaskV2(match.Groups["mask"].Value);
                }
                else
                {
                    var sourceAddress = long.Parse(match.Groups["address"].Value);
                    var value = long.Parse(match.Groups["value"].Value);

                    var addresses = (mask ?? throw new InvalidOperationException("No mask")).Apply(sourceAddress);

                    foreach (var address in addresses)
                    {
                        memory[address] = value;
                    }
                }
            }

            return memory.Sum(x => x.Value);
        }

        public class MaskV2
        {
            private readonly (char c, int i)[] _mask;

            public MaskV2(string mask)
            {
                _mask = mask.Reverse().Select((c, i) => (c, i)).Where(m => m.c != '0').ToArray();
            }

            private static void SetToAllAddresses(int index, bool bit, IEnumerable<BitArray> addresses)
            {
                foreach (var address in addresses)
                {
                    address.Set(index, bit);
                }
            }

            public long[] Apply(long sourceAddress)
            {
                var sourceAddressBytes = BitConverter.GetBytes(sourceAddress);
                var sourceAddressBits = new BitArray(sourceAddressBytes);
                var destinationAddresses = new List<BitArray> {new(sourceAddressBits)};

                foreach (var (c, i) in _mask)
                {
                    switch (c)
                    {
                        case '1':
                            // overwrite with 1
                            SetToAllAddresses(i, true, destinationAddresses);
                            break;
                        case 'X':
                            // Floating: expand out all the current addresses, to provide a true and false for the current index
                            SetToAllAddresses(i, false, destinationAddresses);

                            var newAddresses = new List<BitArray>();
                            foreach (var newAddress in destinationAddresses.Select(currentAddress => new BitArray(currentAddress)))
                            {
                                newAddress.Set(i, true);
                                newAddresses.Add(newAddress);
                            }
                            destinationAddresses.AddRange(newAddresses);
                            break;
                    }
                }

                var bytes = new byte[8];
                return destinationAddresses.Select(destinationAddress =>
                {
                    destinationAddress.CopyTo(bytes, 0);
                    return BitConverter.ToInt64(bytes);
                }).ToArray();
            }
        }
    }
}
