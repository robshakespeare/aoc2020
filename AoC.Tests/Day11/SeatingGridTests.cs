using System;
using System.Linq;
using System.Numerics;
using AoC.Day11;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day11
{
    public class SeatingGridTests
    {
        public const string Example1 = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

        private const string Example1AfterRound1 = @"#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##";

        [Test]
        public void Width_Returns_AsExpected()
        {
            var sut = new SeatingGrid(Example1, default, default);
            sut.Width.Should().Be(10);
        }

        [Test]
        public void CountAdjacentOccupied_Tests()
        {
            var sut1 = new SeatingGrid(Example1, default, default);
            var sut2 = new SeatingGrid(Example1AfterRound1, default, default);

            sut1.CountAdjacentOccupied(0, 0).Should().Be(0);
            sut2.CountAdjacentOccupied(0, 0).Should().Be(2);

            sut1.CountAdjacentOccupied(2, 3).Should().Be(0);
            sut2.CountAdjacentOccupied(2, 3).Should().Be(5);

            sut1.CountAdjacentOccupied(9, 9).Should().Be(0);
            sut2.CountAdjacentOccupied(9, 9).Should().Be(2);

            sut1.CountAdjacentOccupied(10, 10).Should().Be(0);
            sut2.CountAdjacentOccupied(10, 10).Should().Be(1);

            sut1.CountAdjacentOccupied(11, 11).Should().Be(0);
            sut2.CountAdjacentOccupied(11, 11).Should().Be(0);

            sut1.CountAdjacentOccupied(7, 6).Should().Be(0);
            sut2.CountAdjacentOccupied(7, 6).Should().Be(5);
        }

        [Test]
        public void CountVisibleOccupied_Example1()
        {
            var sut = new SeatingGrid(@".......#.
...#.....
.#.......
.........
..#L....#
....#....
.........
#........
...#.....", default, default);

            sut.CountVisibleOccupied(3, 4).Should().Be(8);
        }

        [Test]
        public void CountVisibleOccupied_Example2()
        {
            var sut = new SeatingGrid(@".............
.L.L.#.#.#.#.
.............", default, default);

            sut.CountVisibleOccupied(1, 1).Should().Be(0);
        }

        [Test]
        public void CountVisibleOccupied_Example3()
        {
            var sut = new SeatingGrid(@".##.##.
#.#.#.#
##...##
...L...
##...##
#.#.#.#
.##.##.", default, default);

            sut.CountVisibleOccupied(3, 3).Should().Be(0);
        }

        [Test]
        public void CountVisibleOccupied_Example4()
        {
            var sut = new SeatingGrid(@".##.##.
###.###
##...##
#L.L.#.
##...##
#.#.#.#
.#####.", default, default);

            sut.CountVisibleOccupied(3, 3).Should().Be(4);
        }
    }
}
