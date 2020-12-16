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

        private const string Example1 = @"class: 1-3 or 5-7
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
            part2Result.Should().Be(null);
        }

        [Test]
        public void TrainServiceNotesParsingTest()
        {
            // ACT
            var result = TrainServiceNotes.Parse(Example1);

            // ASSERT
            result.Should().BeEquivalentTo(
                new TrainServiceNotes(
                    new[]
                    {
                        new FieldRule("class", new[] {new Range(1, 3), new Range(5, 7)}),
                        new FieldRule("row", new[] {new Range(6, 11), new Range(33, 44)}),
                        new FieldRule("seat", new[] {new Range(13, 40), new Range(45, 50)})
                    },
                    new Ticket(new[] {7, 1, 14}),
                    new []
                    {
                        new Ticket(new[] {7, 3, 47}),
                        new Ticket(new[] {40, 4, 50}),
                        new Ticket(new[] {55, 2, 20}),
                        new Ticket(new[] {38, 6, 12})
                    }),
                options => options
                    .WithStrictOrdering()
                    .ComparingByMembers<TrainServiceNotes>()
                    .ComparingByMembers<FieldRule>()
                    .ComparingByMembers<Ticket>());
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
