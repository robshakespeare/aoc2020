using AoC.Day6;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day6
{
    public class Day6SolverTests
    {
        private readonly Day6Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"abc

a
b
c

ab
ac

a
a
a
a

b");

            // ASSERT
            part1Result.Should().Be(11);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(6662);
        }

        [Test]
        public void Part2Example()
        {
            // ACT
            var part2Result = _sut.SolvePart2(@"abc

a
b
c

ab
ac

a
a
a
a

b");

            // ASSERT
            part2Result.Should().Be(6);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(3382);
        }
    }
}
