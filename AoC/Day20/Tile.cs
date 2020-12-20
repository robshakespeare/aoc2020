using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day20
{
    public class Tile
    {
        private const int ExpectedTileSize = 10;

        public int TileId { get; }
        public IReadOnlyList<string> Pixels { get; }

        public Tile(int tileId, IReadOnlyList<string> pixels)
        {
            TileId = tileId;
            Pixels = pixels;
        }

        private static readonly Regex TileIdRegex = new(@"Tile (?<tileId>\d+):", RegexOptions.Compiled);

        public static Tile ParseTile(string tileString)
        {
            var match = TileIdRegex.Match(tileString);
            if (!match.Success)
            {
                throw new InvalidOperationException("Invalid tile: " + tileString);
            }

            var tileId = int.Parse(match.Groups["tileId"].Value);
            var pixels = tileString.ReadLines().ToArray()[1..];

            if (pixels.Length != ExpectedTileSize)
            {
                throw new InvalidOperationException($"Tile height must be {ExpectedTileSize}");
            }

            if (pixels.All(line => line.Length != ExpectedTileSize))
            {
                throw new InvalidOperationException($"Tile width must be {ExpectedTileSize}");
            }

            return new Tile(tileId, pixels);
        }

        public static IReadOnlyList<Tile> ParsePuzzleInput(string input) =>
            input.NormalizeLineEndings()
                .Split($"{Environment.NewLine}{Environment.NewLine}")
                .Select(ParseTile)
                .ToArray();
    }
}
