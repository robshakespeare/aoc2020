using System;
using System.Linq;
using System.Numerics;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests
{
    public class MathUtilsTests
    {
        public class TheAngleBetweenMethod
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

        public class TheRotateDirectionMethod
        {
            [Test]
            public void Rotate_East_90DegreesToRight_ShouldBe_South()
            {
                var input = MathUtils.East;

                // ACT
                var result = MathUtils.RotateDirection(input, 90);

                // ASSERT
                result.Should().Be(MathUtils.South);
            }

            [Test]
            public void Rotate_East_360DegreesToRight_ShouldBe_Still_East()
            {
                var input = MathUtils.East;

                // ACT
                var result = MathUtils.RotateDirection(input, 360);

                // ASSERT
                result.Should().Be(MathUtils.East);
            }

            [Test]
            public void Rotate_East_90DegreesToLeft_ShouldBe_North()
            {
                var input = MathUtils.East;

                // ACT
                var result = MathUtils.RotateDirection(input, -90);

                // ASSERT
                result.Should().Be(MathUtils.North);
            }

            [Test]
            public void Rotate_North_180DegreesToLeft_ShouldBe_South()
            {
                var input = MathUtils.North;

                // ACT
                var result = MathUtils.RotateDirection(input, -180);

                // ASSERT
                result.Should().Be(MathUtils.South);
            }

            [Test]
            public void Rotate_North_180DegreesToRight_ShouldBe_South()
            {
                var input = MathUtils.North;

                // ACT
                var result = MathUtils.RotateDirection(input, 180);

                // ASSERT
                result.Should().Be(MathUtils.South);
            }

            [Test]
            public void Rotate_South_270DegreesToRight_ShouldBe_East()
            {
                var input = MathUtils.South;

                // ACT
                var result = MathUtils.RotateDirection(input, 270);

                // ASSERT
                result.Should().Be(MathUtils.East);
            }

            [Test]
            public void Rotate_South_270DegreesToLeft_ShouldBe_West()
            {
                var input = MathUtils.South;

                // ACT
                var result = MathUtils.RotateDirection(input, -270);

                // ASSERT
                result.Should().Be(MathUtils.West);
            }

            [Test]
            public void Rotate_Waypoint_Example1_Test()
            {
                var input = new Vector2(10, -4);

                // ACT
                var result = MathUtils.RotateDirection(input, 90);

                // ASSERT
                result.Should().Be(new Vector2(4, 10));
            }
        }
    }
}
