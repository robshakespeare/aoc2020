using System;
using System.Linq;
using System.Numerics;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests
{
    public class MathUtilsTests
    {
        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 315)]
        [TestCase(1, 0, 270)]
        [TestCase(1, -1, 225)]
        [TestCase(0, -1, 180)]
        [TestCase(-1, -1, 135)]
        [TestCase(-1, 0, 90)]
        [TestCase(-1, 1, 45)]
        public void AngleBetween_Tests(int x, int y, int expectedAngle)
        {
            var angles = Enumerable
                .Range(1, 5)
                .Select(n => new Vector2(x * n * 100, y * n * 100))
                .Select(v =>
                {
                    Console.WriteLine(v);
                    return MathUtils.AngleBetween(Vector2.UnitY, v);
                })
                .ToArray();

            angles.Should().AllBeEquivalentTo(expectedAngle);
        }
    }
}
