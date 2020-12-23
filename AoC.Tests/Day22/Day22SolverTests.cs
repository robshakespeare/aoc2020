using AoC.Day22;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day22
{
    public class Day22SolverTests
    {
        private readonly Day22Solver _sut = new();

        private const string ExampleInput = @"Player 1:
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
10";

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(ExampleInput);

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
        public void Part2Example()
        {
            // ACT
            var part2Result = _sut.SolvePart2(ExampleInput);

            // ASSERT
            part2Result.Should().Be(291);
        }

        [Test]
        [LongRunningTest("Crab Combat Part 2 with real input takes ~1.8 seconds to complete")]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(33651);
        }
    }
}
