using AoC.Day9;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day9
{
    public class XmasCrackerTests
    {
        [Test]
        public void Part1Example()
        {
            const string? input = @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576";

            var sut = XmasCracker.Parse(input, 5);

            // ACT
            var result = sut.GetFirstNumAfterPreambleWhichIsNotSumOfTwoOfPreviousBlock();

            // ASSERT
            result.Should().Be(127);
        }
    }
}
