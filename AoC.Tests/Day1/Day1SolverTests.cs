using AoC.Day1;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day1
{
    public class Day1SolverTests
    {
        private readonly Day1Solver _sut = new Day1Solver();

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(910539);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(116724144);
        }
    }
}
