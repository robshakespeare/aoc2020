using AoC.Day18;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day18
{
    public class ExpressionEvaluatorTests
    {
        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 26)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Evaluate_Expression_Part1TestCases(string expression, long expectedResult)
        {
            ExpressionEvaluator sut = new();

            // ACT
            var result = sut.Evaluate(expression);

            // ASSERT
            result.Should().Be(expectedResult);
        }

        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 231)]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 46)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Evaluate_Expression_Part2TestCases(string expression, long expectedResult)
        {
            ExpressionEvaluator2 sut2 = new();

            // ACT
            var result = sut2.Evaluate(expression);

            // ASSERT
            result.Should().Be(expectedResult);
        }
    }
}
