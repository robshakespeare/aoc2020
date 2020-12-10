using AoC.Day10;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day10
{
    public class JoltageChecker2Tests
    {
        [Test]
        public void Part2Example1()
        {
            const string input = @"16
10
15
5
1
11
7
19
6
12
4";

            // ACT
            var bruteForceAnswer = JoltageChecker.Parse(input).CountDistinctArrangements();
            var part2Result = JoltageChecker2.CountDistinctArrangements(input);

            // ASSERT
            bruteForceAnswer.Should().Be(8);
            part2Result.Should().Be(8);
        }

        [Test]
        public void Part2MyExample1()
        {
            const string input = @"16
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
21";

            // ACT
            var bruteForceAnswer = JoltageChecker.Parse(input).CountDistinctArrangements();
            var part2Result = JoltageChecker2.CountDistinctArrangements(input);

            // ASSERT
            bruteForceAnswer.Should().Be(16);
            part2Result.Should().Be(16);
        }

        [Test]
        public void Part2MyExample2()
        {
            const string input = @"16
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
25";

            // ACT
            var bruteForceAnswer = JoltageChecker.Parse(input).CountDistinctArrangements();
            var part2Result = JoltageChecker2.CountDistinctArrangements(input);

            // ASSERT
            bruteForceAnswer.Should().Be(32);
            part2Result.Should().Be(32);
        }

        [Test]
        public void Part2Example2()
        {
            const string input = @"28
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
3";
            // ACT
            var bruteForceAnswer = JoltageChecker.Parse(input).CountDistinctArrangements();
            var part2Result = JoltageChecker2.CountDistinctArrangements(input);

            // ASSERT
            bruteForceAnswer.Should().Be(19208);
            part2Result.Should().Be(19208);
        }
    }
}
