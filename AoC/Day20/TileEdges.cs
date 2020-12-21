using System.Collections.Generic;

namespace AoC.Day20
{
    public record TileEdges(string Top, string Right, string Bottom, string Left)
    {
        /// <summary>
        /// Returns all of the 4 edges.
        /// The order matches the <see cref="TileEdgeLocation"/> enum.
        /// </summary>
        public IReadOnlyList<string> All { get; } = new []
        {
            Top,
            Right,
            Bottom,
            Left
        };
    }

    /// <summary>
    /// Defines the order of the Tile Edge locations when returned by the <see cref="TileEdges.All"/> method.
    /// </summary>
    public enum TileEdgeLocation
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3
    }
}
