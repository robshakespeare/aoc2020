using System.Collections.Generic;
using System.Linq;
using static AoC.Day20.TileEdgeLocation;

namespace AoC.Day20
{
    /// <summary>
    /// Presents a corner in our "world" coordinates, as in what we see as top right, bottom right, etc...
    /// </summary>
    public class Corner
    {
        public TileEdgeLocation Vertical { get; }

        public TileEdgeLocation Horizontal { get; }

        public int CornerIndex { get; }

        private Corner(TileEdgeLocation vertical, TileEdgeLocation horizontal, int cornerIndex)
        {
            Horizontal = horizontal;
            CornerIndex = cornerIndex;
            Vertical = vertical;
        }

        public static Corner TopRight { get; } = new(Top, Right, 0);
        public static Corner BottomRight { get; } = new(Bottom, Right, 1);
        public static Corner BottomLeft { get; } = new(Bottom, Left, 2);
        public static Corner TopLeft { get; } = new(Top, Left, 3);

        public static IReadOnlyList<Corner> All { get; } = new[] {TopRight, BottomRight, BottomLeft, TopLeft};

        public static IReadOnlyList<int> Indexes { get; } = All.Select(x => x.CornerIndex).ToArray();

        public override string ToString() => $"{Vertical}{Horizontal}";
    }
}
