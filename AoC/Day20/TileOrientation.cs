using System.Collections.Generic;
using System.Linq;

namespace AoC.Day20
{
    public class TileOrientation
    {
        public IReadOnlyList<string> Pixels { get; }
        public Tile Tile { get; }
        public Orientation Orientation { get; }
        public string VisualString { get; }
        public string Name { get; }
        public TileEdges Edges { get; }

        public TileOrientation(Tile tile, Orientation orientation)
        {
            Pixels = orientation.Apply(tile.Pixels);
            Tile = tile;
            Orientation = orientation;
            VisualString = string.Join("\n", Pixels);
            Name = $"{tile.TileId}+{orientation}";

            Edges = new TileEdges(Pixels);
        }

        public override string ToString() => Name;

        /// <summary>
        /// Returns true if this Orientation could fit in the specified corner.
        /// </summary>
        public bool IsOrientationForCorner(Corner corner)
        {
            var grid = Tile.Grid;
            return grid.OuterEdges.Contains(Edges[corner.Horizontal]) &&
                   grid.OuterEdges.Contains(Edges[corner.Vertical]);
        }

        public TileOrientation GetUniquePairToRight() =>
            Tile.Grid.Tiles
                .Where(tile => tile != Tile)
                .SelectMany(tile => tile.TileOrientations)
                .Single(otherTilePerm => Edges[TileEdgeLocation.Right] == otherTilePerm.Edges[TileEdgeLocation.Left]);

        public TileOrientation GetUniquePairToBelow() =>
            Tile.Grid.Tiles
                .Where(tile => tile != Tile)
                .SelectMany(tile => tile.TileOrientations)
                .Single(otherTilePerm => Edges[TileEdgeLocation.Bottom] == otherTilePerm.Edges[TileEdgeLocation.Top]);

        public string[] GetPixelsWithoutBorder()
        {
            return Enumerate().ToArray();

            IEnumerable<string> Enumerate()
            {
                var rows = Pixels.ToArray()[1..^1];
                foreach (var row in rows)
                {
                    yield return row[1..^1];
                }
            }
        }
    }
}
