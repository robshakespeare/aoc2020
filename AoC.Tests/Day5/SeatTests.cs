using System;
using AoC.Day5;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

// ReSharper disable StringLiteralTypo
namespace AoC.Tests.Day5
{
    public class SeatTests
    {
        [Test]
        public void SeatIdTests()
        {
            var seat1 = new Seat(1, 0);
            var seat2 = new Seat(1, 1);
            var seat3 = new Seat(1, 2);
            var seat4 = new Seat(2, 0);

            TestContext.WriteLine(string.Join(Environment.NewLine, seat1, seat2, seat3, seat4));

            seat1.Id.Should().BeLessThan(seat2.Id);
            seat2.Id.Should().BeLessThan(seat3.Id);
            seat3.Id.Should().BeLessThan(seat4.Id);
        }

        [Test]
        public void Parse_Example1Test()
        {
            var seat = Seat.Parse("FBFBBFFRLR");

            // ASSERT
            using (new AssertionScope())
            {
                seat.Row.Should().Be(44);
                seat.Column.Should().Be(5);
                seat.Id.Should().Be(357);
            }
        }

        [TestCase("BFFFBBFRRR", 70, 7, 567)]
        [TestCase("FFFBBBFRRR", 14, 7, 119)]
        [TestCase("BBFFBBFRLL", 102, 4, 820)]
        public void Parse_OtherExamplesTest(string ticketRef, int expectedRow, int expectedColumn, int expectedSeatId)
        {
            var seat = Seat.Parse(ticketRef);

            // ASSERT
            using (new AssertionScope())
            {
                seat.Row.Should().Be(expectedRow);
                seat.Column.Should().Be(expectedColumn);
                seat.Id.Should().Be(expectedSeatId);
            }
        }
    }
}
