using AoC.Day16;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day16
{
    public class Day16SolverTests
    {
        private readonly Day16Solver _sut = new();

        public const string Example1 = @"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12";

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(Example1);

            // ASSERT
            part1Result.Should().Be(71);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(23115);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(239727793813);
        }
    }
}
