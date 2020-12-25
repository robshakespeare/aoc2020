using AoC.Day25;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day25
{
    public class Day25SolverTests
    {
        private readonly Day25Solver _sut = new();

        [TestCase(5764801, 8)]
        [TestCase(17807724, 11)]
        public void DetermineLoopSize_Tests(int publicKey, int expectedLoopSize)
        {
            Day25Solver.DetermineLoopSize(publicKey).Should().Be(expectedLoopSize);
        }

        [TestCase(17807724, 8, 14897079)]
        [TestCase(5764801, 11, 14897079)]
        public void TransformSubjectNumber_Tests(int subjectNumber, int loopSize, int expectedEncryptionKey)
        {
            Day25Solver.TransformSubjectNumber(subjectNumber, loopSize).Should().Be(expectedEncryptionKey);
        }

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"5764801
17807724");

            // ASSERT
            part1Result.Should().Be(14897079);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(1478097);
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().StartWith("Day 25, part 2, was free :)");
        }
    }
}
