using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using static AoC.Day24.LobbyLayout.TileColorFacingUp;

namespace AoC.Day24
{
    public class LobbyLayout
    {
        public enum TileColorFacingUp
        {
            White,
            Black, // Note: the first 'flip' is actually 'to black', because tiles "start with the white side facing up"
            OutOfBounds
        }

        private static readonly Vector2 ReferenceTile = new(0, 0);

        private readonly Dictionary<Vector2, TileColorFacingUp> _tiles;

        public LobbyLayout(Dictionary<Vector2, TileColorFacingUp> tiles) => _tiles = tiles;

        public IEnumerable<KeyValuePair<Vector2, TileColorFacingUp>> Tiles => _tiles;

        public long CountTilesBlackSideUp() => _tiles.Count(x => x.Value == Black);

        private TileColorFacingUp GetTile(Vector2 position) => _tiles.TryGetValue(position, out var tile) ? tile : OutOfBounds;

        private static IEnumerable<Vector2> GetAdjacentPositions(Vector2 currentPosition) => Directions.Select(dir => currentPosition + dir);

        private static Vector2 ParseDirection(string direction) =>
            direction switch
            {
                //"nw" => new Vector2(-0.5f, -0.5f),
                //"ne" => new Vector2(0.5f, -0.5f),
                //"e" => new Vector2(1, 0),
                //"se" => new Vector2(0.5f, 0.5f),
                //"sw" => new Vector2(-0.5f, 0.5f),
                //"w" => new Vector2(-1, 0),

                "nw" => new Vector2(-1f, -1f),
                "ne" => new Vector2(1f, -1f),
                "e" => new Vector2(2, 0),
                "se" => new Vector2(1f, 1f),
                "sw" => new Vector2(-1f, 1f),
                "w" => new Vector2(-2, 0),

                _ => throw new InvalidOperationException("Invalid direction: " + direction)
            };

        private static readonly IReadOnlyList<Vector2> Directions = new[] {"se", "sw", "nw", "ne", "e", "w"}.Select(ParseDirection).ToArray();

        private static readonly Regex ParseDirectionsRegex = new("se|sw|nw|ne|e|w|.", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static LobbyLayout ParsePuzzleInput(string puzzleInput)
        {
            var directionsList = puzzleInput.ReadLines().ToArray();

            Dictionary<Vector2, TileColorFacingUp> tiles = new();

            foreach (var directions in directionsList)
            {
                var position = ParseDirectionsRegex.Matches(directions)
                    .Select(match => ParseDirection(match.Value))
                    .Aggregate(ReferenceTile, (current, direction) => current + direction);

                if (tiles.TryGetValue(position, out var existing))
                {
                    tiles[position] = existing == Black ? White : Black; // Flip it
                }
                else
                {
                    tiles.Add(position, Black); // Place new tile, Black facing up
                }
            }

            return new LobbyLayout(tiles);
        }

        public static LobbyLayout SimulateLivingArtExhibit(string puzzleInput, int numberOfDays)
        {
            var applyGrowth = false;
            
            var lobbyLayout = ParsePuzzleInput(puzzleInput);

            for (var dayNum = 1; dayNum <= numberOfDays; dayNum++)
            {
                Dictionary<Vector2, TileColorFacingUp> newTiles = new();

                var growth = new List<Vector2>();

                // Update existing tiles
                foreach (var (tilePosition, existingTileColor) in lobbyLayout.Tiles)
                {
                    var (newTileColor, adjacentTiles) = GetNewTileColor(tilePosition, lobbyLayout, existingTileColor);

                    newTiles.Add(tilePosition, newTileColor);

                    growth.AddRange(adjacentTiles.Where(x => x.color == OutOfBounds).Select(x => x.position));
                }

                // Apply growth
                if (applyGrowth)
                {
                    foreach (var growthPosition in growth.Distinct())
                    {
                        //var (newTileColor, _) = GetNewTileColor(growthPosition, lobbyLayout, existingTileColor: Black);

                        newTiles.Add(growthPosition, Black); //newTileColor);
                    }
                }

                lobbyLayout = new LobbyLayout(newTiles);
            }

            return lobbyLayout;
        }

        private static (TileColorFacingUp newTileColor, IReadOnlyList<(Vector2 position, TileColorFacingUp color)> adjacentTiles) GetNewTileColor(
            Vector2 tilePosition,
            LobbyLayout lobbyLayout,
            TileColorFacingUp existingTileColor)
        {
            var adjacentTiles = GetAdjacentPositions(tilePosition)
                .Select(position => (position, color: lobbyLayout.GetTile(position)))
                .ToArray();

            //var adjacentPositions = GetAdjacentPositions(tilePosition).ToArray();

            var countOfBlackAdjacentTiles = adjacentTiles.Count(x => x.color == Black); //adjacentPositions.Select(lobbyLayout.GetTile).Count(color => color == Black);
            var newTileColor = existingTileColor switch
            {
                Black when countOfBlackAdjacentTiles is 0 or > 2 => White,
                White when countOfBlackAdjacentTiles is 2 => Black,
                _ => existingTileColor
            };
            return (newTileColor, adjacentTiles);
        }
    }
}
