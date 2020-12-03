using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day3
{
    public class Day3Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input)
        {
            var direction = new Vector2Int(3, 1);
            var grid = new Grid(input);
            return CountTreesEncountered(grid, direction);
        }

        private static long? CountTreesEncountered(Grid grid, Vector2Int direction)
        {
            var position = direction;
            var trees = new List<Vector2Int>();

            while (position.Y < grid.NumLines)
            {
                if (grid.IsTree(position))
                {
                    trees.Add(position);
                }

                position += direction;
            }

            return trees.Count;
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }

        public record Grid(string PuzzleInput)
        {
            private readonly GridLine[] _lines = PuzzleInput.ReadAllLines().Select(line => new GridLine(line)).ToArray();

            public int NumLines => _lines.Length;

            public bool IsTree(Vector2Int position)
            {
                if (position.Y >= NumLines)
                {
                    throw new InvalidOperationException($"Position {position} is BELOW bottom of grid.");
                }

                return _lines[position.Y].IsTree(position);
            }
        }

        public record GridLine(string Line)
        {
            public bool IsTree(Vector2Int position)
            {
                return Line[position.X % Line.Length] == '#';
            }
        }
    }
}
