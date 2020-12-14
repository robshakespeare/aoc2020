using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests
{
    public class Vector2IntTests
    {
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(0, 0, 0, 1, 1)]
        [TestCase(0, 0, 0, -1, 1)]
        [TestCase(0, 0, 2, 3, 5)]
        [TestCase(0, 0, -2, -3, 5)]
        [TestCase(1, 6, -1, 5, 3)]
        [TestCase(2, 3, -1, 5, 5)]
        [TestCase(2, 3, 1, 6, 4)]
        [TestCase(2, 3, 3, 5, 3)]
        public void ManhattanDistance_Tests(int x1, int y1, int x2, int y2, int expectedResult)
        {
            var vectorA = new Vector2Int(x1, y1);
            var vectorB = new Vector2Int(x2, y2);

            // ACT
            var result = Vector2Int.ManhattanDistance(vectorA, vectorB);

            // ASSERT
            result.Should().Be(expectedResult);
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 1, 1)]
        [TestCase(0, -1, 1)]
        [TestCase(2, 3, 5)]
        [TestCase(-2, -3, 5)]
        [TestCase(10, 16, 26)]
        [TestCase(-10, 16, 26)]
        [TestCase(10, -16, 26)]
        [TestCase(-10, -16, 26)]
        public void ManhattanDistanceFromZero_Tests(int x, int y, int expectedResult)
        {
            var vector = new Vector2Int(x, y);

            // ACT
            var result = vector.ManhattanDistanceFromZero;

            // ASSERT
            result.Should().Be(expectedResult);
        }
    }
}
