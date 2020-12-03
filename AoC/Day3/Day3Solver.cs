using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day3
{
    public class Day3Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input)
        {
            var grid = new Grid(input);
            var direction = new Vector2Int(3, 1);
            return grid.CountTreesEncountered(direction);
        }

        protected override long? SolvePart2Impl(string input)
        {
            var grid = new Grid(input);
            return new[]
                {
                    new Vector2Int(1, 1),
                    new Vector2Int(3, 1),
                    new Vector2Int(5, 1),
                    new Vector2Int(7, 1),
                    new Vector2Int(1, 2)
                }
                .Select(grid.CountTreesEncountered)
                .Aggregate(1L, (accumulate, current) => accumulate * current);
        }

        public record Grid(string PuzzleInput)
        {
            private readonly GridLine[] _lines = PuzzleInput.ReadAllLines().Select(line => new GridLine(line)).ToArray();

            public int NumLines => _lines.Length;

            public long CountTreesEncountered(Vector2Int direction)
            {
                var position = direction;
                var trees = new List<Vector2Int>();

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
