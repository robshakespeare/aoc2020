using System;
using AoC.Day2;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day2
{
    public class PasswordLineTests
    {
        public class TheParseMethod
        {
            [TestCase("1-3 a: abcde", 1, 3, 'a', "abcde")]
            [TestCase("4-9 z: lztmfrqqz", 4, 9, 'z', "lztmfrqqz")]
            [TestCase("1-2 c: d", 1, 2, 'c', "d")]
            [TestCase("1-3 1: 123", 1, 3, '1', "123")]
            [TestCase("1-3 #: £$%", 1, 3, '#', "£$%")]
            [TestCase("1-3  a: abcde", 1, 3, 'a', "abcde")]
            [TestCase("1-3 a:  abcde", 1, 3, 'a', "abcde")]
            [TestCase("1-3   a:   abcde", 1, 3, 'a', "abcde")]
            public void ParsesValidInputs(
                string inputLine,
                int expectedLowerBound,
                int expectedUpperBound,
                char expectedRequiredChar,
                string expectedPassword)
            {
                // ACT
                var result = PasswordLine.Parse(inputLine);

                // ASSERT
                result.Should().BeEquivalentTo(
                    new PasswordLine(expectedLowerBound, expectedUpperBound, expectedRequiredChar, expectedPassword));
            }

            [Test]
            public void ThrowsArgumentNullExWhenInputIsNull()
            {
                Action action = () => PasswordLine.Parse(null!);

                // ACT & ASSERT
                action.Should().Throw<ArgumentNullException>()
                    .And
                    .ParamName.Should().Be("line");
            }

            [TestCase("")]
            [TestCase(" ")]
            [TestCase(" \t ")]
            [TestCase("a1-3 a: abcde")]
            [TestCase("a-3 a: abcde")]
            [TestCase("1-a a: abcde")]
            [TestCase("1-3 : abcde")]
            [TestCase("1-3 a abcde")]
            [TestCase("1-3 a:abcde")]
            [TestCase("1-3a: abcde")]
            [TestCase("1-3 a: ")]
            [TestCase("1 a: abcde")]
            [TestCase("1- a: abcde")]
            [TestCase("-1 a: abcde")]
            [TestCase("- a: abcde")]
            [TestCase("-1-3 a: abcde")]
            [TestCase("1.1-3 a: abcde")]
            [TestCase("1-3a:abcde")]
            public void ThrowsForInvalidInputs(string inputLine)
            {
                Action action = () => PasswordLine.Parse(inputLine);

                // ACT & ASSERT
                action.Should().Throw<InvalidOperationException>()
                    .WithMessage("Invalid password line: " + inputLine);
            }
        }
    }
}
