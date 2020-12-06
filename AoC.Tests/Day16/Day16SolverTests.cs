using AoC.Day16;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day16
{
    public class Day16SolverTests
    {
        private readonly Day16Solver _sut = new();

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
