using AoC.Day1;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day1
{
    public class Day1SolverTests
    {
        private readonly Day1Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"1721
979
366
299
675
1456");

            // ASSERT
            part1Result.Should().Be(514579);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(910539);
        }

        [Test]
        public void Part2Example()
        {
            // ACT
            var part1Result = _sut.SolvePart2(@"1721
979
366
299
675
1456");

            // ASSERT
            part1Result.Should().Be(241861950);
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
