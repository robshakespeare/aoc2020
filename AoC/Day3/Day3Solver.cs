using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC.Day3
{
    public class Day3Solver : SolverBase
    {
        public override string DayName => "Toboggan Trajectory";

        protected override long? SolvePart1Impl(string input)
        {
            var grid = new Grid(input);
            var direction = new Vector2(3, 1);
            return grid.CountTreesEncountered(direction);
        }

        protected override long? SolvePart2Impl(string input)
        {
            var grid = new Grid(input);
            var directions = new[]
            {
                new Vector2(1, 1),
                new Vector2(3, 1),
                new Vector2(5, 1),
                new Vector2(7, 1),
                new Vector2(1, 2)
            };
            return directions
                .Select(grid.CountTreesEncountered)
                .Aggregate(1L, (accumulate, current) => accumulate * current);
        }

        public record Grid(string PuzzleInput)
        {
            private readonly GridLine[] _lines = PuzzleInput.ReadLines().Select(line => new GridLine(line)).ToArray();

            public int NumLines => _lines.Length;

            public long CountTreesEncountered(Vector2 direction)
            {
                var position = direction;
                var trees = new List<Vector2>();

                while (position.Y < NumLines)
                {
                    if (IsTree(position))
                    {
                        trees.Add(position);
                    }

                    position += direction;
                }

                return trees.Count;
            }

            public bool IsTree(Vector2 position) =>
                position.Y >= NumLines
                    ? throw new InvalidOperationException($"Position {position} is BELOW bottom of grid.")
                    : _lines[position.Y.Round()].IsTree(position);
        }

        public record GridLine(string Line)
        {
            public bool IsTree(Vector2 position) => Line[position.X.Round() % Line.Length] == '#';
        }
    }
}
