using System;
using System.Collections.Generic;
using System.Numerics;

namespace AoC.Day20
{
    public class TilePerm
    {
        public IReadOnlyList<string> Pixels { get; }
        public Tile Tile { get; }
        public Orientation Orientation { get; }
        public string Id { get; }
        public string Name { get; }
        public TileEdges Edges { get; }

        public TilePerm(Tile tile, Orientation orientation)
        {
            Pixels = BuildPixels(tile, orientation.Rotation, orientation.Scale);
            Tile = tile;
            Orientation = orientation;
            Id = string.Join("\n", Pixels);
            Name = $"{tile.TileId}+{orientation}";

            Edges = new TileEdges(Pixels);
        }

        public override string ToString() => Name;

        private static IReadOnlyList<string> BuildPixels(Tile tile, Rotation rotation, Scale scale)
        {
            var rotated = MathUtils.RotateGrid(tile.Pixels, 90 * (int) rotation);

            var scaleBy = scale switch
            {
                Scale.None => new Vector2(1, 1),
                Scale.FlipHorizontal => new Vector2(1, -1),
                Scale.FlipVertical => new Vector2(-1, 1),
                _ => throw new InvalidOperationException("Invalid scale: " + scale)
            };

            return MathUtils.ScaleGrid(rotated, scaleBy);
        }

        /// <summary>
        /// Returns true if this permutation could fit in the specified corner.
        /// </summary>
        public bool IsPermForCorner(Corner corner)
        {
            var grid = Tile.Grid;
            return grid.OuterEdges.Contains(Edges[corner.Horizontal]) &&
                   grid.OuterEdges.Contains(Edges[corner.Vertical]);
        }
    }

    // ReSharper disable UnusedMember.Global
    public enum Rotation
    {
        Zero = 0,
        Right90 = 1,
        Right180 = 2,
        Right270 = 3
    }

    // ReSharper disable UnusedMember.Global
    public enum Scale
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVertical = 2
    }
}
