using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            var center = new Vector2(x, y);

            var adjacentPositions = new[]
            {
                center + new Vector2(-1, -1),
                center + new Vector2(0, -1),
                center + new Vector2(1, -1),

                center + new Vector2(-1, 0),
                center + new Vector2(1, 0),

                center + new Vector2(-1, 1),
                center + new Vector2(0, 1),
                center + new Vector2(1, 1)
            };

            return adjacentPositions.Select(GetSeat).Count(seat => seat == Occupied);
        }

        private char GetSeat(Vector2 position)
        {
            var y = (int)Math.Round(position.Y);
            if (y < 0 || y >= _lines.Count)
            {
                return Floor;
            }

            var line = _lines[y];

            var x = (int)Math.Round(position.X);
            if (x < 0 || x >= line.Length)
            {
                return Floor;
            }

            return line[x];
        }

        public override string ToString() => Grid;
    }
}
