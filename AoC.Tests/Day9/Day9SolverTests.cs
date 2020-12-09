using AoC.Day9;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day9
{
    public class Day9SolverTests
    {
        private readonly Day9Solver _sut = new();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(23278925);
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
