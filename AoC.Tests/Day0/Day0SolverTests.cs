using AoC.Day0;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day0
{
    public class Day0SolverTests
    {
        private readonly Day0Solver _sut = new Day0Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(37);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(112);
        }
    }
}
