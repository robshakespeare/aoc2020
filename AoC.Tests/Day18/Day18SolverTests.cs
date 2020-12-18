using AoC.Day18;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day18
{
    public class Day18SolverTests
    {
        private readonly Day18Solver _sut = new();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(8298263963837);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(145575710203332);
        }
    }
}
