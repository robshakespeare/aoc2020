using System;
using System.Linq;
using AoC.Day13;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day13
{
    public class Day13SolverTests
    {
        private readonly Day13Solver _sut = new();

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"939
7,13,x,x,59,x,31,19");

            // ASSERT
            part1Result.Should().Be(295);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(4938);
        }

        [TestCase(@"939
7,13,x,x,59,x,31,19", 1068781)]
        [TestCase("17,x,13,19", 3417)]
        [TestCase("67,7,59,61", 754018)]
        [TestCase("67,x,7,59,61", 779210)]
        [TestCase("67,7,x,59,61", 1261476)]
        [TestCase("1789,37,47,1889", 1202161486)]
        public void Part2Examples(string input, long expectedResult)
        {
            // ACT
            var part2ResultA = Day13Solver.GetMatchingDepartureTimesBruteForce(input);
            var part2ResultB = Day13Solver.GetMatchingDepartureTimesEfficient2(input);

            // ASSERT
            part2ResultA.Should().Be(expectedResult);
            part2ResultB.Should().Be(expectedResult);
        }

        [Test]
        public void Part2SimpleExample()
        {
            // ACT
            var part2Result = Day13Solver.GetMatchingDepartureTimesEfficient2("17,x,13,19");

            // ASSERT
            part2Result.Should().Be(3417);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(230903629977901);
        }
    }
}
