using System.Linq;

namespace AoC.Day1
{
    public class Day1Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input)
        {
            var inputNumbers = input.ReadAllLines().Select(int.Parse).ToArray();

            var resultPair = inputNumbers
                .SelectMany((a, i) => inputNumbers
                    .Select((b, j) => new {a = (a, i), b = (b, j)})
                    .Where(x => x.a.i != x.b.j))
                .First(x => x.a.a + x.b.b == 2020);

            return resultPair.a.a * resultPair.b.b;
        }

        protected override long? SolvePart2Impl(string input)
        {
            var inputNumbers = input.ReadAllLines().Select(int.Parse).ToArray();

            var resultTriple = inputNumbers
                .SelectMany((a, i) => inputNumbers
                    .SelectMany((b, j) => inputNumbers
                        .Select((c, k) => new { a = (a, i), b = (b, j), c = (c, k) }))
                    .Where(x => x.a.i != x.b.j && x.a.i != x.c.k))
                .First(x => x.a.a + x.b.b + x.c.c == 2020);

            return resultTriple.a.a * resultTriple.b.b * resultTriple.c.c;
        }
    }
}
