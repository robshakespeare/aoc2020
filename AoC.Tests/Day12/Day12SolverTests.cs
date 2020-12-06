using AoC.Day12;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day12
{
    public class Day12SolverTests
    {
        private readonly Day12Solver _sut = new();

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
