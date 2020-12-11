using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Day11
{
    public class SeatingGrid
    {
        private const char Floor = '.';
        private const char Empty = 'L';
        private const char Occupied = '#';

        private readonly IReadOnlyList<string> _lines;

        public string Grid { get; }

        public int Width { get; }

        public SeatingGrid(string input) : this(
            input.ReadLines().ToArray(),
            input.TakeWhile(c => c != '\r' && c != '\n').Count())
        {
        }

        private SeatingGrid(IReadOnlyList<string> lines, int width)
        {
            _lines = lines;
            Grid = string.Join(Environment.NewLine, _lines);
            Width = width;
        }

        public long CountOccupiedSeats() => _lines.SelectMany(line => line).Count(seat => seat == Occupied);

        public SeatingGrid GenerateNextGrid()
        {
            StringBuilder newLine = new();
            List<string> newLines = new();

            foreach (var (line, y) in _lines.Select((line, y) => (line, y)))
            {
                newLine.Clear();

                foreach (var (seat, x) in line.Select((seat, x) => (seat, x)))
                {
                    var newSeat = seat switch
                    {
                        Empty when CountAdjacentOccupied(x, y) == 0 => Occupied,
                        Occupied when CountAdjacentOccupied(x, y) >= 4 => Empty,
                        _ => seat
                    };
                    newLine.Append(newSeat);
                }

                newLines.Add(newLine.ToString());
            }

            return new SeatingGrid(newLines, Width);
        }

        public int CountAdjacentOccupied(int x, int y)
        {
            var center = new Vector2Int(x, y);

            var adjacentPositions = new[]
            {
                center + new Vector2Int(-1, -1),
                center + new Vector2Int(0, -1),
                center + new Vector2Int(1, -1),

                center + new Vector2Int(-1, 0),
                center + new Vector2Int(1, 0),

                center + new Vector2Int(-1, 1),
                center + new Vector2Int(0, 1),
                center + new Vector2Int(1, 1)
            };

            return adjacentPositions.Select(GetSeat).Count(seat => seat == Occupied);
        }

        private char GetSeat(Vector2Int position)
        {
            if (position.Y < 0 || position.Y >= _lines.Count)
            {
                return Floor;
            }

            var line = _lines[position.Y];

            if (position.X < 0 || position.X >= line.Length)
            {
                return Floor;
            }

            return line[position.X];
        }

        public override string ToString() => Grid;
    }
}
