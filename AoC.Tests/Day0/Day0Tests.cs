using AoC.Day0;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day0
{
    public class Day0Tests // rs-todo: finish, quickly testing Vector2 at the same time
    {
        private readonly Day0Solver _sut = new Day0Solver();

        [Test]
        public void Part1ReTest()
        {
            _sut.Run();
        }

        //[Test]
        //public void Part1ReTest()
        //{
        //    // ACT
        //    var part1Result = _sut.SolvePart1();

        //    // ASSERT
        //    part1Result.Should().Be(3465245);
        //}

        //[Test]
        //public void Part2ReTest()
        //{
        //    // ACT
        //    var part2Result = _sut.SolvePart2();

        //    // ASSERT
        //    part2Result.Should().Be(5194970);
        //}
    }
}
