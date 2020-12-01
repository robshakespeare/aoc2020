using System;
using System.Linq;
using System.Numerics;

namespace AoC.Day0
{
    public class Day0Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input)
        {
            var vectors = input
                .ReadAllLines()
                .Select(line => line.Split(","))
                .Select(parts => new Vector2(float.Parse(parts[0]), float.Parse(parts[1])))
                .ToArray();

            return (long)Math.Round(Vector2.Distance(vectors[0], vectors[1]));
        }

        protected override long? SolvePart2Impl(string input)
        {
            var vectors = input
                .ReadAllLines()
                .Select(line => line.Split(","))
                .Select(parts => new Vector2(float.Parse(parts[0]), float.Parse(parts[1])))
                .ToArray();

            return (long)Math.Round(
                Vector2.Distance(vectors[0], vectors[1]) +
                Vector2.Distance(vectors[2], vectors[3]) +
                Vector2.Distance(vectors[4], vectors[5]));
        }
    }
}
