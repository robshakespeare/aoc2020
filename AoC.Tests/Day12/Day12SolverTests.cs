using AoC.Day12;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day12
{
    public class Day12SolverTests
    {
        private readonly Day12Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"F10
N3
F7
R90
F11");

            // ASSERT
            part1Result.Should().Be(25);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(362);
        }

        [Test]
        public void Part2Example()
        {
            // ACT
            var part2Result = _sut.SolvePart2(@"F10
N3
F7
R90
F11");

            // ASSERT
            part2Result.Should().Be(286);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(29895);
        }
    }
}
