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
        public Grid Grid { get; }
        public IReadOnlyList<TileEdgePerm> TileEdgePerms { get; }

        private readonly TileEdges _edges;
        private readonly Lazy<IReadOnlyList<string>> _outerEdges;

        public Tile(int tileId, IReadOnlyList<string> pixels, Grid grid)
        {
            TileId = tileId;
            Pixels = pixels;
            Grid = grid;
            _edges = new TileEdges(
                Top: pixels[0],
                Right: string.Join("", pixels.Select(line => line[^1])),
                Bottom: pixels[^1],
                Left: string.Join("", pixels.Select(line => line[0])));

            TileEdgePerms = Orientation.Permutations.Select(orientation => new TileEdgePerm(this, orientation)).ToArray();

            _outerEdges = new Lazy<IReadOnlyList<string>>(() => _edges.All.Where(Grid.OuterEdges.Contains).ToArray());
        }

        /// <summary>
        /// Returns the 4 original edges of this tile.
        /// </summary>
        public TileEdges GetEdges() => _edges;

        /// <summary>
        /// Returns the original edges of this tile which are outer edges to the whole Grid.
        /// </summary>
        public IReadOnlyList<string> OuterEdges => _outerEdges.Value;

        public int NumOuterEdges => OuterEdges.Count;

        public bool IsOuterEdgeCornerTile => NumOuterEdges == 2;

        public bool IsOuterEdgeNonCornerTile => NumOuterEdges == 1;

        private static readonly Regex TileIdRegex = new(@"Tile (?<tileId>\d+):", RegexOptions.Compiled);

        public static Tile ParseTile(string tileString, Grid grid)
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

            return new Tile(tileId, pixels, grid);
        }

        /// <summary>
        /// Returns the permutations that could fit in the specified corner.
        /// </summary>
        public IEnumerable<TileEdgePerm> GetPermsForCorner(Corner corner) => TileEdgePerms.Where(perm => perm.IsPermForCorner(corner));
    }
}
