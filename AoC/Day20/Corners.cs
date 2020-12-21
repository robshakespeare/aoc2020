using System.Collections.Generic;
using static AoC.Day20.TileEdgeLocation;

namespace AoC.Day20
{
    /// <summary>
    /// Presents a corner in our "world" coordinates, as in what we see as top right, bottom right, etc...
    /// </summary>
    public class Corner
    {
        public TileEdgeLocation Horizontal { get; }
        public TileEdgeLocation Vertical { get; }

        private Corner(TileEdgeLocation vertical, TileEdgeLocation horizontal)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public static Corner TopRight { get; } = new(Top, Right);
        public static Corner BottomRight { get; } = new(Bottom, Right);
        public static Corner BottomLeft { get; } = new(Bottom, Left);
        public static Corner TopLeft { get; } = new(Top, Left);

        public static IReadOnlyList<Corner> All { get; } = new[] {TopRight, BottomRight, BottomLeft, TopLeft};

        public override string ToString() => $"{Vertical}{Horizontal}";
    }
}
