using AoC.Day7;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day7
{
    public class Day7SolverTests
    {
        private readonly Day7Solver _sut = new Day7Solver();

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
