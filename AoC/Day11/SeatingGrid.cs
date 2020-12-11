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
        private readonly int _occupancyThreshold;
        private readonly bool _visibilityOccupancy;
        private readonly Func<int, int, int> _countOccupancy;

        public string Grid { get; }

        public int Width { get; }

        public SeatingGrid(string input, int occupancyThreshold, bool visibilityOccupancy) : this(
            input.ReadLines().ToArray(),
            input.TakeWhile(c => c != '\r' && c != '\n').Count(),
            occupancyThreshold,
            visibilityOccupancy)
        {
        }

        private SeatingGrid(IReadOnlyList<string> lines, int width, int occupancyThreshold, bool visibilityOccupancy)
        {
            _lines = lines;
            _occupancyThreshold = occupancyThreshold;
            _visibilityOccupancy = visibilityOccupancy;
            _countOccupancy = visibilityOccupancy ? CountVisibleOccupied : CountAdjacentOccupied;
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
                        Empty when _countOccupancy(x, y) == 0 => Occupied,
                        Occupied when _countOccupancy(x, y) >= _occupancyThreshold => Empty,
                        _ => seat
                    };
                    newLine.Append(newSeat);
                }

                newLines.Add(newLine.ToString());
            }

            return new SeatingGrid(newLines, Width, _occupancyThreshold, _visibilityOccupancy);
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

        private static readonly HashSet<double> VisibilityAngles = new(new[]
        {
            MathUtils.AngleBetween(Vector2.UnitY, new Vector2(-1, -1)),
            MathUtils.AngleBetween(Vector2.UnitY, new Vector2(0, -1)),
            MathUtils.AngleBetween(Vector2.UnitY, new Vector2(1, -1)),

            MathUtils.AngleBetween(Vector2.UnitY, new Vector2(-1, 0)),
            MathUtils.AngleBetween(Vector2.UnitY, new Vector2(1, 0)),

            MathUtils.AngleBetween(Vector2.UnitY, new Vector2(-1, 1)),
            MathUtils.AngleBetween(Vector2.UnitY, new Vector2(0, 1)),
            MathUtils.AngleBetween(Vector2.UnitY, new Vector2(1, 1))
        });

        public int CountVisibleOccupied(int ix, int iy)
        {
            var center = new Vector2(ix, iy);

            var allOtherSeats = _lines.SelectMany((line, y) => line.Select((seat, x) => seat != Floor ? new {seat, position = new Vector2(x, y)} : null))
                .Where(x => x != null && !x.position.Equals(center))
                .Select(seat => seat!);

            return allOtherSeats
                .Select(x =>
                {
                    var viewVector = x.position - center;
                    return new
                    {
                        angle = MathUtils.AngleBetween(Vector2.UnitY, viewVector),
                        viewVector,
                        distance = viewVector.Length(),
                        x.seat,
                        x.position
                    };
                })
                .Where(x => VisibilityAngles.Contains(x.angle))
                .GroupBy(x => x.angle)
                .Select(grp => grp.OrderBy(x => x.distance).First())
                .Count(x => x.seat == Occupied);
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
