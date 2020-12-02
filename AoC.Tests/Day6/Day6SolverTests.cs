using AoC.Day6;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day6
{
    public class Day6SolverTests
    {
        private readonly Day6Solver _sut = new Day6Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(null);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(null);
        }
    }
}
