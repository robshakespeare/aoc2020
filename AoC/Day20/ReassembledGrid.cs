using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC.Day20
{
    public class ReassembledGrid
    {
        private const char SeaMonsterMark = 'O';
        private const char GeneralMark = '#';

        private readonly IReadOnlyList<char[]> _rows;
        private readonly string _grid;

        public ReassembledGrid(string grid)
        {
            _grid = grid;
            _rows = grid
                .ReadLines()
                .Select(line => line.ToCharArray())
                .ToArray();
        }

        public override string ToString() => _grid;

        public long CountMarksThatAreNotSeaMonsters()
        {
            for (var y = 0; y < _rows.Count; y++)
            {
                var row = _rows[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var offset = new Vector2(x, y);

                    foreach (var seaMonsterCoords in SeaMonsterOrientationsCoords)
                    {
                        if (IsSeaMonster(seaMonsterCoords, offset))
                        {
                            MarkSeaMonster(seaMonsterCoords, offset);
                        }
                    }
                }
            }

            var gridWithSeaMonsters = string.Join(Environment.NewLine, _rows.Select(row => new string(row)));

            Console.WriteLine("gridWithSeaMonsters:");
            Console.WriteLine(gridWithSeaMonsters);

            return _rows.SelectMany(row => row).Count(chr => chr == GeneralMark);
        }

        private bool IsSeaMonster(Vector2[] seaMonsterCoords, Vector2 offset) =>
            seaMonsterCoords
                .Select(pos => pos + offset)
                .All(pos => GetTile(pos) is GeneralMark or SeaMonsterMark);

        private void MarkSeaMonster(Vector2[] seaMonsterCoords, Vector2 offset)
        {
            foreach (var pos in seaMonsterCoords
                .Select(pos => pos + offset))
            {
                _rows[pos.Y.Round()][pos.X.Round()] = SeaMonsterMark;
            }
        }

        private char GetTile(Vector2 position)
        {
            var y = position.Y.Round();
            if (y < 0 || y >= _rows.Count)
            {
                return ' ';
            }

            var row = _rows[y];

            var x = position.X.Round();
            if (x < 0 || x >= row.Length)
            {
                return ' ';
            }

            return row[x];
        }

        private static IReadOnlyList<string> SeaMonster { get; } = new[]
        {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   "
        };

        private static IReadOnlyList<string>[] SeaMonsterOrientations { get; } = Orientation.Permutations
            .Select(orientation => orientation.Apply(SeaMonster))
            .ToArray();

        private static IReadOnlyList<Vector2[]> SeaMonsterOrientationsCoords { get; } = SeaMonsterOrientations
            .Select(seaMonsterOrientation => seaMonsterOrientation
                .SelectMany((line, y) => line
                    .Select((chr, x) => (chr, x, y))
                    .Where(t => t.chr == GeneralMark)
                    .Select(t => new Vector2(t.x, t.y)))
                .ToArray())
            .ToArray();
    }
}
