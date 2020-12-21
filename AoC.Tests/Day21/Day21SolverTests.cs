using AoC.Day21;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable StringLiteralTypo
namespace AoC.Tests.Day21
{
    public class Day21SolverTests
    {
        private readonly Day21Solver _sut = new();

        private const string ExampleInput = @"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)";

        [Test]
        public void Part1Example()
        {
            // ACT
            var part1Result = _sut.SolvePart1(ExampleInput);

            // ASSERT
            part1Result.Should().Be(5);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(2317);
        }

        [Test]
        public void Part2Example()
        {
            // ACT
            var part2Result = _sut.SolvePart2(ExampleInput);

            // ASSERT
            part2Result.Should().Be("mxmxvkd,sqjhc,fvjkl");
        }

        [Test]
        public void Part2ReTest()
        {
            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be("kbdgs,sqvv,slkfgq,vgnj,brdd,tpd,csfmb,lrnz");
        }
    }
}
