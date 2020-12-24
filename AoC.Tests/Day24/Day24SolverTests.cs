using AoC.Day24;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day24
{
    public class Day24SolverTests
    {
        private readonly Day24Solver _sut = new();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(263);
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
