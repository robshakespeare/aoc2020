using AoC.Day13;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day13
{
    public class Day13SolverTests
    {
        private readonly Day13Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"939
7,13,x,x,59,x,31,19");

            // ASSERT
            part1Result.Should().Be(295);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(4938);
        }

        [Test]
        public void Part2Example()
        {
            // ACT
            var part1Result = _sut.SolvePart2(@"939
7,13,x,x,59,x,31,19");

            // ASSERT
            part1Result.Should().Be(1068788);
        }

        [Test]
        public void Part2Example2()
        {
            // ACT
            var part1Result = _sut.SolvePart2(@"17,x,13,19");

            // ASSERT
            part1Result.Should().Be(1068788);
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
