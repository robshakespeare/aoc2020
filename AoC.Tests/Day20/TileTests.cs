using System;
using System.Linq;
using AoC.Day20;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day20
{
    public class TileTests
    {
        [Test]
        public void ParsePuzzleInputTest()
        {
            var puzzleInput = new InputLoader(20).PuzzleInputPart1;

            // ACT
            var result = Tile.ParsePuzzleInput(puzzleInput);

            // ASSERT
            result.Count.Should().Be(144); // i.e. we have 144 tiles in total
            Math.Sqrt(result.Count).Should().Be(12); // i.e. we're looking for a 12 x 12 grid

            result.First().TileEdgePerms.Count.Should().Be(4 * 3); // numberOfRotations * numberOfScales

            Console.WriteLine(ObjectDumper.Dump(result.First()));
        }

        [Test]
        public void PermsTest()
        {
            Tile.Perms.Count.Should().Be(4 * 3); // numberOfRotations * numberOfScales
        }
    }
}
