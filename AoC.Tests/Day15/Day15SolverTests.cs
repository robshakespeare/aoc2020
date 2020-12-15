using AoC.Day15;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day15
{
    public class Day15SolverTests
    {
        private readonly Day15Solver _sut = new();

        [TestCase("0,3,6", 436)]
        [TestCase("1,3,2", 1)]
        [TestCase("2,1,3", 10)]
        [TestCase("1,2,3", 27)]
        [TestCase("2,3,1", 78)]
        [TestCase("3,2,1", 438)]
        [TestCase("3,1,2", 1836)]
        public void Part1Examples(string input, int expectedResult)
        {
            // ACT
            var part1Result = _sut.SolvePart1(input);

            // ASSERT
            part1Result.Should().Be(expectedResult);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(1665);
        }

        [TestCase("0,3,6", 175594)]
        [TestCase("1,3,2", 2578)]
        [Ignore("Currently takes ~8 seconds per test case, there must be an efficient way to do it!")]
        public void Part2Examples(string input, int expectedResult)
        {
            // ACT
            var part2Result = _sut.SolvePart2(input);

            // ASSERT
            part2Result.Should().Be(expectedResult);
        }

        [Test]
        [Ignore("Currently takes ~8 seconds, there must be an efficient way to do it!")]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(16439);
        }
    }
}
