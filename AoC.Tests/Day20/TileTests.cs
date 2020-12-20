using System;
using System.Linq;
using AoC.Day20;
using FluentAssertions;
using MoreLinq;
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
            var (resultTiles, resultGridSize) = Tile.ParsePuzzleInput(puzzleInput);

            // ASSERT
            resultTiles.Count.Should().Be(144); // i.e. we have 144 tiles in total
            Math.Sqrt(resultTiles.Count).Should().Be(12); // i.e. we're looking for a 12 x 12 grid
            resultGridSize.Should().Be(12); // i.e. same as above, we're looking for a 12 x 12 grid
            (resultGridSize * resultGridSize).Should().Be(resultTiles.Count);

            resultTiles.First().TileEdgePerms.Count.Should().Be(4 * 3); // numberOfRotations * numberOfScales

            Console.WriteLine(ObjectDumper.Dump(resultTiles.First()));
        }

        [Test]
        public void TilePermsTest()
        {
            Tile.Perms.Count.Should().Be(4 * 3); // numberOfRotations * numberOfScales
        }
    }
}
