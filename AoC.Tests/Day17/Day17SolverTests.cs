using AoC.Day17;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day17
{
    public class Day17SolverTests
    {
        private readonly Day17Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@".#.
..#
###");

            // ASSERT
            part1Result.Should().Be(112);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(223);
        }

        [Test]
        [LongRunningTest("~600ms")]
        public void Part2Example()
        {
            // ACT
            var part2Result = _sut.SolvePart2(@".#.
..#
###");

            // ASSERT
            part2Result.Should().Be(848);
        }

        [Test]
        [LongRunningTest("~1sec")]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(1884);
        }

        [Test]
        public void DirectionsCountsAreAsExpected()
        {
            PocketDimension3d.Directions.Count.Should().Be(26);
            PocketDimension4d.Directions.Count.Should().Be(80);
        }
    }
}
