using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day20
{
    public record TileEdges(string Top, string Right, string Bottom, string Left)
    {
        public TileEdges(IReadOnlyList<string> pixels) : this(
            Top: pixels[0],
            Right: string.Join("", pixels.Select(line => line[^1])),
            Bottom: pixels[^1],
            Left: string.Join("", pixels.Select(line => line[0])))
        {
        }

        /// <summary>
        /// Returns all of the 4 edges.
        /// The order matches the <see cref="TileEdgeLocation"/> enum.
        /// </summary>
        public IEnumerable<string> All()
        {
            yield return Top;
            yield return Right;
            yield return Bottom;
            yield return Left;
        }

        /// <summary>
        /// Returns the edge at the specified location (top/right/bottom/left).
        /// </summary>
        public string this[TileEdgeLocation location] => location switch
        {
            TileEdgeLocation.Top => Top,
            TileEdgeLocation.Right => Right,
            TileEdgeLocation.Bottom => Bottom,
            TileEdgeLocation.Left => Left,
            _ => throw new InvalidOperationException("Invalid location: " + location)
        };
    }

    public enum TileEdgeLocation
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3
    }
}
