using AoC.Day3;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day3
{
    public class Day3SolverTests
    {
        private readonly Day3Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#");

            // ASSERT
            part1Result.Should().Be(7);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(289);
        }

        [Test]
        public void Part2Example()
        {
            // ACT
            var part1Result = _sut.SolvePart2(@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#");

            // ASSERT
            part1Result.Should().Be(336);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(5522401584);
        }
    }
}
