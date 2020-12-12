using AoC.Day12;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day12
{
    public class ShipTests
    {
        public class TheRotateDirectionMethod
        {
            [Test]
            public void Rotate_East_90DegreesToRight_ShouldBe_South()
            {
                var input = Ship.East;

                // ACT
                var result = Ship.RotateDirection(input, 90);

                // ASSERT
                result.Should().Be(Ship.South);
            }

            [Test]
            public void Rotate_East_360DegreesToRight_ShouldBe_Still_East()
            {
                var input = Ship.East;

                // ACT
                var result = Ship.RotateDirection(input, 360);

                // ASSERT
                result.Should().Be(Ship.East);
            }

            [Test]
            public void Rotate_East_90DegreesToLeft_ShouldBe_North()
            {
                var input = Ship.East;

                // ACT
                var result = Ship.RotateDirection(input, -90);

                // ASSERT
                result.Should().Be(Ship.North);
            }

            [Test]
            public void Rotate_North_180DegreesToLeft_ShouldBe_South()
            {
                var input = Ship.North;

                // ACT
                var result = Ship.RotateDirection(input, -180);

                // ASSERT
                result.Should().Be(Ship.South);
            }

            [Test]
            public void Rotate_North_180DegreesToRight_ShouldBe_South()
            {
                var input = Ship.North;

                // ACT
                var result = Ship.RotateDirection(input, 180);

                // ASSERT
                result.Should().Be(Ship.South);
            }

            [Test]
            public void Rotate_South_270DegreesToRight_ShouldBe_East()
            {
                var input = Ship.South;

                // ACT
                var result = Ship.RotateDirection(input, 270);

                // ASSERT
                result.Should().Be(Ship.East);
            }

            [Test]
            public void Rotate_South_270DegreesToLeft_ShouldBe_West()
            {
                var input = Ship.South;

                // ACT
                var result = Ship.RotateDirection(input, -270);

                // ASSERT
                result.Should().Be(Ship.West);
            }
        }
    }
}
