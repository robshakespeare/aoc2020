using System.Linq;

namespace AoC.Day20
{
    public class TileEdgePerm
    {
        public Tile Tile { get; }
        public Rotation Rotation { get; }
        public Scale Scale { get; }
        public TileEdges Edges { get; }

        public TileEdgePerm(Tile tile, Rotation rotation, Scale scale)
        {
            Tile = tile;
            Rotation = rotation;
            Scale = scale;

            Edges = BuildEdges(tile, rotation, scale);
        }

        private static TileEdges BuildEdges(Tile tile, Rotation rotation, Scale scale)
        {
            // Apply rotation
            var (top, bottom, left, right) = RotateNumberOfTimes((int)rotation, tile.GetEdges());

            // Apply scale (flip)
            if (scale == Scale.FlipHorizontal)
            {
                var oldTop = top;
                top = bottom;
                bottom = oldTop;

                left = new string(left.Reverse().ToArray());
                right = new string(right.Reverse().ToArray());
            }
            else if (scale == Scale.FlipVertical)
            {
                top = new string(top.Reverse().ToArray());
                bottom = new string(bottom.Reverse().ToArray());

                var oldLeft = left;
                left = right;
                right = oldLeft;
            }

            return new TileEdges(Top: top, Bottom: bottom, Left: left, Right: right);
        }

        private static TileEdges RotateNumberOfTimes(int times, TileEdges edges)
        {
            var (top, bottom, left, right) = edges;

            for (var i = 0; i < times; i++)
            {
                var oldTop = top;
                var oldBottom = bottom;
                var oldLeft = left;
                var oldRight = right;

                right = oldTop; // Right = Top
                bottom = new string(oldRight.Reverse().ToArray()); // Bottom = Right reversed
                left = oldBottom; // Left = Bottom
                top = oldLeft; // Top = Left
            }

            return new TileEdges(Top: top, Bottom: bottom, Left: left, Right: right);
        }
    }

    public enum Rotation
    {
        Zero = 0,
        Right90 = 1,
        Right180 = 2,
        Right270 = 3
    }

    public enum Scale
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVertical = 2
    }
}
