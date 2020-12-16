using System;
using AoC.Day16;
using FluentAssertions;
using NUnit.Framework;
using static AoC.Day16.Day16Solver;

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

        [Test]
        public void FieldRule_IsValidValue_Tests()
        {
            var sut = new FieldRule("test", new[]
            {
                new Range(1, 3),
                new Range(5, 7)
            });

            // ASSERT
            sut.IsValidValue(1).Should().BeTrue();
            sut.IsValidValue(2).Should().BeTrue();
            sut.IsValidValue(3).Should().BeTrue();

            sut.IsValidValue(5).Should().BeTrue();
            sut.IsValidValue(6).Should().BeTrue();
            sut.IsValidValue(7).Should().BeTrue();

            sut.IsValidValue(4).Should().BeFalse();
            sut.IsValidValue(0).Should().BeFalse();
            sut.IsValidValue(8).Should().BeFalse();
            sut.IsValidValue(100).Should().BeFalse();
        }
    }
}
