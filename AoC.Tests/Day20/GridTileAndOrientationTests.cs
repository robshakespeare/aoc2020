using System;
using System.Linq;
using AoC.Day20;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day20
{
    public class GridTileAndOrientationTests
    {
        private static readonly string Day20PuzzleInput = new InputLoader(20).PuzzleInputPart1;

        [Test]
        public void ParsePuzzleInputTest()
        {
            // ACT
            var grid = Grid.ParsePuzzleInput(Day20PuzzleInput);

            // ASSERT
            grid.Tiles.Count.Should().Be(144); // i.e. we have 144 tiles in total
            Math.Sqrt(grid.Tiles.Count).Should().Be(12); // i.e. we're looking for a 12 x 12 grid
            grid.GridSize.Should().Be(12); // i.e. same as above, we're looking for a 12 x 12 grid
            (grid.GridSize * grid.GridSize).Should().Be(grid.Tiles.Count);

            grid.Tiles.First().TileEdgePerms.Count.Should().Be(4 * 3); // numberOfRotations * numberOfScales

            var numCornerTiles = grid.OuterEdgeCornerTiles.Count;
            numCornerTiles.Should().Be(4);

            var numOuterEdgeNonCornerTiles = grid.OuterEdgeNonCornerTiles.Count;

            var gridSize = grid.GridSize;
            var expectedNumOfOuterEdgeTiles = gridSize * 2 + (gridSize - 2) * 2; // Formula for getting expected number of edge tiles including corners

            (numCornerTiles + numOuterEdgeNonCornerTiles).Should().Be(expectedNumOfOuterEdgeTiles);

            Console.WriteLine(new {gridSize, numCornerTiles, numOuterEdgeNonCornerTiles, total = expectedNumOfOuterEdgeTiles});
        }

        [Test]
        public void Orientation_Permutations_Count_Test()
        {
            Orientation.Permutations.Count.Should().Be(4 * 3); // numberOfRotations * numberOfScales
        }

        [Test]
        public void Corner_All_Order_Tests()
        {
            string.Join(", ", Corner.All).Should().Be("TopRight, BottomRight, BottomLeft, TopLeft");
            string.Join(", ", Corner.All.OrderBy(x => x.CornerIndex)).Should().Be("TopRight, BottomRight, BottomLeft, TopLeft");
            Corner.Indexes.Should().BeEquivalentTo(new[] { 0, 1, 2, 3 }, options => options.WithStrictOrdering());
        }
    }
}
