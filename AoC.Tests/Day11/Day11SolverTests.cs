using AoC.Day11;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day11
{
    public class Day11SolverTests
    {
        private readonly Day11Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(SeatingGridTests.Example1);

            // ASSERT
            part1Result.Should().Be(37);
        }

        [Test]
        [Ignore("Takes ~300ms, too long")]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(2113);
        }

        [Test]
        public void Part2Example1()
        {
            // ACT
            var part2Result = _sut.SolvePart2(SeatingGridTests.Example1);

            // ASSERT
            part2Result.Should().Be(26);
        }

        [Test]
        [Ignore("Takes 9 minutes!, waaaaay too long!! Maybe I'll have time to come back and do this in not such a brute force way!")]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(1865);
        }
    }
}
