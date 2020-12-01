using AoC.Day4;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day4
{
    public class Day4SolverTests
    {
        private readonly Day4Solver _sut = new Day4Solver();

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
