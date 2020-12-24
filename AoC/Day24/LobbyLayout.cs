using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC.Day24
{
    public class LobbyLayout
    {
        private const bool Black = false;
        private const bool White = true;
        private const bool StartingColor = White; // Note: tiles "start with the white side facing up"

        private static readonly Vector2 ReferenceTile = new(0, 0);

        private readonly Dictionary<Vector2, bool> _tiles;

        public LobbyLayout(Dictionary<Vector2, bool> tiles) => _tiles = tiles;

        public long NumOfTilesBlackSideUp => _tiles.Count(x => x.Value == Black);

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

                tiles[position] = !GetTileColor(position, tiles); // Note: the `!` flips the color
            }

            return new LobbyLayout(tiles);
        }

        private bool GetTileColor(Vector2 position) => GetTileColor(position, _tiles);

        // ReSharper disable once SimplifyConditionalTernaryExpression
        private static bool GetTileColor(Vector2 position, IDictionary<Vector2, bool> tiles) =>
            tiles.TryGetValue(position, out var tile) ? tile : StartingColor;

        /// <summary>
        /// The tile floor in the lobby is meant to be a living art exhibit. Every day, the tiles are all flipped according to the following rules:
        ///  * Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white.
        ///  * Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black.
        /// </summary>
        public static LobbyLayout SimulateLivingArtExhibit(string puzzleInput, int numberOfDays)
        {
            var lobbyLayout = ParsePuzzleInput(puzzleInput);

            for (var dayNum = 1; dayNum <= numberOfDays; dayNum++)
            {
                lobbyLayout = lobbyLayout.NextGeneration();
            }

            return lobbyLayout;
        }

        private LobbyLayout NextGeneration()
        {
            var newTiles = new Dictionary<Vector2, bool>();
            var growthPositions = new HashSet<Vector2>();

            // Update existing tiles
            foreach (var (tilePosition, existingTileColor) in _tiles)
            {
                var (newTileColor, adjacentPositions) = GetNewTileColor(tilePosition, existingTileColor);

                newTiles.Add(tilePosition, newTileColor);

                foreach (var growthPosition in adjacentPositions
                    .Where(adjacentPosition => !_tiles.ContainsKey(adjacentPosition)))
                {
                    growthPositions.Add(growthPosition);
                }
            }

            // Apply growth
            foreach (var growthPosition in growthPositions)
            {
                var (newTileColor, _) = GetNewTileColor(growthPosition, existingTileColor: StartingColor);
                newTiles.Add(growthPosition, newTileColor);
            }

            return new LobbyLayout(newTiles);
        }

        private static readonly IReadOnlyList<Vector2> Directions = new[] {"se", "sw", "nw", "ne", "e", "w"}.Select(ParseDirection).ToArray();

        private (bool newTileColor, IReadOnlyList<Vector2> adjacentPositions) GetNewTileColor(
            Vector2 tilePosition,
            bool existingTileColor)
        {
            var adjacentPositions = Directions.Select(dir => tilePosition + dir).ToArray();

            var countOfBlackAdjacentTiles = adjacentPositions.Select(GetTileColor).Count(color => color == Black);
            var newTileColor = existingTileColor switch
            {
                Black when countOfBlackAdjacentTiles is 0 or > 2 => White,
                White when countOfBlackAdjacentTiles is 2 => Black,
                _ => existingTileColor
            };

            return (newTileColor, adjacentPositions);
        }
    }
}
