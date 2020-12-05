using AoC.Day5;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day5
{
    public class Day5SolverTests
    {
        private readonly Day5Solver _sut = new();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(871);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(640);
        }
    }
}
