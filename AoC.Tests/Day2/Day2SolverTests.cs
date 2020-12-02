using AoC.Day2;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day2
{
    public class Day2SolverTests
    {
        private readonly Day2Solver _sut = new Day2Solver();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc");

            // ASSERT
            part1Result.Should().Be(2);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(383);
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
