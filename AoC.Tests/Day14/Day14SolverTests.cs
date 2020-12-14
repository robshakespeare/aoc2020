using AoC.Day14;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day14
{
    public class Day14SolverTests
    {
        private readonly Day14Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0");

            // ASSERT
            part1Result.Should().Be(165);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(11884151942312);
        }

        [Test]
        public void Part2Example()
        {
            // ACT
            var part2Result = _sut.SolvePart2(@"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1");

            // ASSERT
            part2Result.Should().Be(208);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(2625449018811);
        }
    }
}
