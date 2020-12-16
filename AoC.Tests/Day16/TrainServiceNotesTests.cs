using System;
using AoC.Day16;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;

namespace AoC.Tests.Day16
{
    public class TrainServiceNotesTests
    {
        private static EquivalencyAssertionOptions<TExpectation> Equivalency<TExpectation>(EquivalencyAssertionOptions<TExpectation> options) =>
            options.WithStrictOrdering()
                .ComparingByMembers<TrainServiceNotes>()
                .ComparingByMembers<FieldRule>()
                .ComparingByMembers<Ticket>();

        [Test]
        public void ParsingTest()
        {
            // ACT
            var result = TrainServiceNotes.Parse(Day16SolverTests.Example1);

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
                    new[]
                    {
                        new Ticket(new[] {7, 3, 47}),
                        new Ticket(new[] {40, 4, 50}),
                        new Ticket(new[] {55, 2, 20}),
                        new Ticket(new[] {38, 6, 12})
                    }),
                Equivalency);
        }

        [Test]
        public void ParsingDoesSupportSpacesInFieldName()
        {
            const string input = @"departure time: 1-3 or 5-7

your ticket:
7,1,14

nearby tickets:
7,3,47";

            // ACT
            var result = TrainServiceNotes.Parse(input);

            // ASSERT
            result.Rules.Should().BeEquivalentTo(
                new[]
                {
                    new FieldRule("departure time", new[] {new Range(1, 3), new Range(5, 7)}),
                },
                Equivalency);
        }

        [Test]
        public void GetValidTicketsTest()
        {
            // ACT
            var result = TrainServiceNotes.Parse(Day16SolverTests.Example1).GetValidTickets();

            // ASSERT
            result.Should().BeEquivalentTo(
                new[]
                {
                    new Ticket(new[] {7, 1, 14}), // Your ticket is included
                    new Ticket(new[] {7, 3, 47})
                },
                Equivalency);
        }
    }
}
