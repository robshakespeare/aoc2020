using AoC.Day23;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day23
{
    public class Day23SolverTests
    {
        private readonly Day23Solver _sut = new();

        [Test]
        public void Part1Example_10Moves()
        {
            var crabCupsGame = new CrabCupsGame("389125467", false);

            // ACT
            crabCupsGame.Play(10);
            var part1Result = crabCupsGame.GetCupOrder();

            // ASSERT
            part1Result.Should().Be(92658374);
        }

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1("389125467");

            // ASSERT
            part1Result.Should().Be(67384529);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(95648732);
        }

        [Test]
        [Ignore("Takes ~2.8 seconds")]
        public void Part2Example()
        {
            // ACT
            var part2Result = _sut.SolvePart2("389125467");

            // ASSERT
            part2Result.Should().Be(149245887792);
        }

        [Test]
        [Ignore("Takes ~3 seconds")]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(192515314252);
        }
    }
}
