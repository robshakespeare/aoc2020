using System;
using System.Linq;
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
        public void Part2ExampleMe1()
        {
            // ACT
            var part1Result = JoltageChecker.Parse(@"16
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
21
22
25").CountDistinctArrangements();

            // ASSERT
            part1Result.Should().Be(32);
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
        public void Part2ExamineTestGapsOf1()
        {
            Enumerable.Range(1, 10).Select(i =>
            {
                var csv = string.Join(", ", Enumerable.Range(1, i));
                Console.WriteLine(csv);

                Console.Write($"{i} +0:");
                JoltageChecker.ParseCsv(csv).CountDistinctArrangementsBruteForce();
                Console.WriteLine("");
                return 0;
            }).ToArray();
            return;

            Console.WriteLine("-----");

            Enumerable.Range(1, 10).Select(i =>
            {
                var range = Enumerable.Range(2, i).ToArray();
                var max = range.Max();
                var csv = string.Join(", ", range.Concat(new[] { max + 2, max + 4, max + 6 }));
                Console.WriteLine(csv);

                Console.Write($"{i} +1:");
                JoltageChecker.ParseCsv(csv).CountDistinctArrangementsBruteForce();
                return 0;
            }).ToArray();

            Console.WriteLine("-----");

            Enumerable.Range(1, 10).Select(i =>
            {
                var range = Enumerable.Range(2, i).ToArray();
                var max = range.Max();
                var csv = string.Join(", ", range.Concat(new[] { max + 2 }));
                Console.WriteLine(csv);

                Console.Write($"{i} +1:");
                JoltageChecker.ParseCsv(csv).CountDistinctArrangementsBruteForce();
                return 0;
            }).ToArray();
            return;

            Console.Write("2 +0:");
            JoltageChecker.ParseCsv("2, 3").CountDistinctArrangementsBruteForce();

            Console.Write("3 +0:");
            JoltageChecker.ParseCsv("2, 3, 4").CountDistinctArrangementsBruteForce();

            Console.Write("4 +0:");
            JoltageChecker.ParseCsv("2, 4, 6, 7, 8, 9, 12, 15, 17").CountDistinctArrangementsBruteForce();

            Console.Write("4 +0:");
            JoltageChecker.ParseCsv("2, 4, 6, 8, 9, 10, 11, 14, 17, 19").CountDistinctArrangementsBruteForce();

            Console.Write("5 +0:");
            JoltageChecker.ParseCsv("2, 4, 6, 7, 8, 9, 10, 13, 16, 18").CountDistinctArrangementsBruteForce();

            Console.Write("4 +1:");
            JoltageChecker.ParseCsv("2, 4, 6, 7, 8, 9, 11, 14, 16").CountDistinctArrangementsBruteForce();

            Console.Write("5 +1:");
            JoltageChecker.ParseCsv("2, 4, 6, 7, 8, 9, 10, 12, 15, 17").CountDistinctArrangementsBruteForce();
            return;

            JoltageChecker.ParseCsv("1, 2, 3").CountDistinctArrangementsBruteForce();

            JoltageChecker.ParseCsv("1, 2, 3, 4").CountDistinctArrangementsBruteForce();

            JoltageChecker.ParseCsv("1, 2, 3, 4, 5").CountDistinctArrangementsBruteForce();

            JoltageChecker.ParseCsv("1, 2, 3, 4, 5, 6").CountDistinctArrangementsBruteForce();

            JoltageChecker.ParseCsv("1, 3, 5, 7, 10, 12").CountDistinctArrangementsBruteForce();

            JoltageChecker.ParseCsv("1, 2, 3, 5, 7, 10, 12").CountDistinctArrangementsBruteForce();

            var sut = JoltageChecker.ParseCsv("1, 2, 3, 5, 7, 10, 12");
            var correctAnswer = sut.CountDistinctArrangementsBruteForce();
            var optimalAnswer = sut.CountDistinctArrangements();

            ////optimalAnswer.Should().Be(correctAnswer);
        }

        [Test]
        public void Part2ExamineTest()
        {
            var sut = JoltageChecker.ParseCsv("2, 4, 6, 8, 10, 11, 12, 13, 16, 17, 18, 20");
            var correctAnswer = sut.CountDistinctArrangementsBruteForce();
            var optimalAnswer = sut.CountDistinctArrangements();

            ////optimalAnswer.Should().Be(correctAnswer);
        }

        [Test]
        public void Part2ReTest()
        {

            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(null);
        }
    }
}
