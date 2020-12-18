using AoC.Day18;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day18
{
    public class ExpressionEvaluatorTests
    {
        private static readonly ExpressionEvaluator Sut = new();

        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 26)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Evaluate_Expression_Part1TestCases(string expression, long expectedResult)
        {
            // ACT
            var result = Sut.Evaluate(expression);

            // ASSERT
            result.Should().Be(expectedResult);
        }
    }
}
