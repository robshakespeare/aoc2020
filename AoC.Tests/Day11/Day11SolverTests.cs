using AoC.Day11;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day11
{
    public class Day11SolverTests
    {
        private readonly Day11Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(SeatingGridTests.Example1);

            // ASSERT
            part1Result.Should().Be(37);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(2113);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(0);
        }
    }
}
