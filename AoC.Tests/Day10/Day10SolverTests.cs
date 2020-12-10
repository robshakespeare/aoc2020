using System;
using AoC.Day10;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day10
{
    public class Day10SolverTests
    {
        private readonly Day10Solver _sut = new();

        [Test]
        public void Part1Example1()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"16
10
15
5
1
11
7
19
6
12
4");

            // ASSERT
            part1Result.Should().Be(7 * 5);
        }

        [Test]
        public void Part1Example2()
        {
            // ACT
            var part1Result = _sut.SolvePart1(@"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3");

            // ASSERT
            part1Result.Should().Be(22 * 10);
        }

        [Test]
        public void Part1ReTest()
        {
            // ACT
            var part1Result = _sut.SolvePart1();

            // ASSERT
            part1Result.Should().Be(2210);
        }

        [Test]
        public void Part2Example1()
        {
            // ACT
            var part1Result = _sut.SolvePart2(@"16
10
15
5
1
11
7
19
6
12
4");

            // ASSERT
            part1Result.Should().Be(8);
        }

        [Test]
        public void Part2ExampleMe1()
        {
            // ACT
            var part1Result = _sut.SolvePart2(@"16
10
15
5
1
11
7
19
6
12
4
20
21");

            // ASSERT
            part1Result.Should().Be(16);
        }

        [Test]
        public void Part2Example2()
        {
            // ACT
            var part1Result = _sut.SolvePart2(@"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3");

            // ASSERT
            part1Result.Should().Be(19208);
        }

        [Test]
        public void Part2ReTest()
        {
            throw new InvalidOperationException("need to do it properly!");

            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(null);
        }
    }
}
