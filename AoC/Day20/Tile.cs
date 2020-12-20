using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day20
{
    public class Tile
    {
        private const int ExpectedTileSize = 10;

        public static readonly IReadOnlyList<(Rotation rotation, Scale scale)> Perms =
            Enum.GetValues(typeof(Rotation)).Cast<Rotation>()
                .SelectMany(rotation => Enum.GetValues(typeof(Scale)).Cast<Scale>().Select(scale => (rotation, scale)))
                .ToArray();

        public int TileId { get; }
        public IReadOnlyList<string> Pixels { get; }
        public IReadOnlyList<TileEdgePerm> TileEdgePerms { get; }

        private readonly TileEdges _edges;

        public Tile(int tileId, IReadOnlyList<string> pixels)
        {
            TileId = tileId;
            Pixels = pixels;
            _edges = new TileEdges(
                Top: pixels[0],
                Bottom: pixels[^1],
                Left: string.Join("", pixels.Select(line => line[0])),
                Right: string.Join("", pixels.Select(line => line[^1])));

            TileEdgePerms = Perms.Select(perm => new TileEdgePerm(this, perm.rotation, perm.scale)).ToArray();
        }

        public TileEdges GetEdges() => _edges;

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

        public static (IReadOnlyList<Tile> tiles, int gridSize) ParsePuzzleInput(string input)
        {
            var tiles = input.NormalizeLineEndings()
                .Split($"{Environment.NewLine}{Environment.NewLine}")
                .Select(ParseTile)
                .ToArray();

            var gridSize = (int)Math.Sqrt(tiles.Length);

            return (tiles, gridSize);
        }
    }
}
