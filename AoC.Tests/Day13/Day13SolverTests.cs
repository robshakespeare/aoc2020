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

        [Test]
        public void ExtendedGcdTests()
        {
            Day13Solver.ExtendedGcd(5, 13);
        }

        [Test]
        public void ModInverseTest()
        {
            //Console.WriteLine(Day13Solver.ModInverse(33, 7) * 5); <- looks good

            //Console.WriteLine(Day13Solver.ModInverse(57, 19)); // * 3);

            

            Console.WriteLine(Day13Solver.SolveBigN(247, 17, 0));
            Console.WriteLine(Day13Solver.SolveBigN(323, 13, 2));
            Console.WriteLine(Day13Solver.SolveBigN(221, 19, 3));

            //Console.WriteLine(SolveBigN(247, 17, 1));
            //Console.WriteLine(SolveBigN(323, 13, 3));
            //Console.WriteLine(SolveBigN(221, 19, 4));

            //SolveBigN(5, 7, 3).Should().Be(2);

            //SolveBigN(5, 7, 1).Should().Be(3);

            //SolveBigN(33, 7, 5).Should().Be(12);

            return;

            long SolveNv2(int a, int m, int n)
            {
                return (long)Day13Solver.ModInverse(a, m) * n;
            }

            long SolveNv3(int a, int m, int n)
            {
                var (gcd, s, t) = Day13Solver.ExtendedGcd(a, m);

                //Console.WriteLine($"n({a} * X) mod {m} = {n}");
                //Console.WriteLine($"{(gcd, s, t)}");

                // rs-todo: include the check and throw?
                //if (gcd != 1)
                //{
                //    Console.WriteLine($"Values are not co-prime, cannot invert! GCD = {gcd}");
                //}
                //else
                //{
                //    Console.WriteLine($"  Inverse = {t}");
                //    Console.WriteLine($"  X = Inverse * n = {t * n}");
                //    Console.WriteLine($"  {a * t * n} mod {m} = {(a * t * n) % m}");
                //}

                return t * n;
            }

            //Console.WriteLine(SolveBigN(33, 7, 5));
            //Console.WriteLine(SolveNv2(33, 7, 5));

            //Console.WriteLine(SolveNv3(33, 7, 5));

            //Console.WriteLine(SolveNv2(21, 11, 8));

            Console.WriteLine(SolveNv3(247, 17, 0));
            Console.WriteLine(SolveNv3(323, 13, 2));
            Console.WriteLine(SolveNv3(221, 19, 3));

            Console.WriteLine(SolveNv3(247, 17, 1));
            Console.WriteLine(SolveNv3(323, 13, 3));
            Console.WriteLine(SolveNv3(221, 19, 4));
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
        public void Part2MyVerySimpleExample()
        {
            Day13Solver.arrow_alignment(9, 15, 3).Should().Be(18);

            // ACT
            var part2Result = Day13Solver.GetMatchingDepartureTimesEfficient("7,13");

            // ASSERT
            part2Result.Should().Be(null);
        }

        [Test]
        public void Part2SimpleExample()
        {
            // ACT
            var part2Result = Day13Solver.GetMatchingDepartureTimesEfficient("17,x,13,19");

            // ASSERT
            part2Result.Should().Be(3417);
        }

        [Test]
        public void Part2ReTest()
        {
            //throw new NotImplementedException("rs-todo: make it run in time!");

            // ACT
            var part2Result = _sut.SolvePart2();

            // ASSERT
            part2Result.Should().Be(230903629977901);
        }
    }
}
