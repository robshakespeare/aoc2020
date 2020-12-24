using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC.Day24
{
    public class LobbyLayout
    {
        private const bool Black = false; // Note: the first 'flip' is actually 'to black', because tiles "start with the white side facing up"
        private const bool White = true;

        private static readonly Vector2 ReferenceTile = new(0, 0);

        private readonly Dictionary<Vector2, bool> _tiles;
        private IReadOnlyList<string> _directionsList;

        public LobbyLayout(Dictionary<Vector2, bool> tiles, IReadOnlyList<string> directionsList)
        {
            _tiles = tiles;
            _directionsList = directionsList;
        }

        public long CountTilesBlackSideUp() => _tiles.Count(x => x.Value == Black);

        private static Vector2 ParseDirection(string direction) =>
            direction switch
            {
                "nw" => new Vector2(-0.5f, -0.5f),
                "ne" => new Vector2(0.5f, -0.5f),
                "e" => new Vector2(1, 0),
                "se" => new Vector2(0.5f, 0.5f),
                "sw" => new Vector2(-0.5f, 0.5f),
                "w" => new Vector2(-1, 0),
                _ => throw new InvalidOperationException("Invalid direction: " + direction)
            };

        private static readonly Regex ParseDirectionsRegex = new("se|sw|nw|ne|e|w|.", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        
        public static LobbyLayout ParsePuzzleInput(string puzzleInput)
        {
            var directionsList = puzzleInput.ReadLines().ToArray();

            Dictionary<Vector2, bool> tiles = new();

            foreach (var directions in directionsList)
            {
                var position = ParseDirectionsRegex.Matches(directions)
                    .Select(match => ParseDirection(match.Value))
                    .Aggregate(ReferenceTile, (current, direction) => current + direction);

                if (tiles.TryGetValue(position, out var existing))
                {
                    tiles[position] = !existing; // Flip it
                }
                else
                {
                    tiles.Add(position, Black); // Place new tile, Black facing up
                }
            }

            return new LobbyLayout(tiles, directionsList);
        }
    }
}
