using AoC.Day22;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day22
{
    public class Day22SolverTests
    {
        private readonly Day22Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10");

            // ASSERT
            part1Result.Should().Be(306);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(33421);
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
