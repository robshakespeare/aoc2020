using AoC.Day8;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day8
{
    public class Day8SolverTests
    {
        private readonly Day8Solver _sut = new();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(1137);
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
