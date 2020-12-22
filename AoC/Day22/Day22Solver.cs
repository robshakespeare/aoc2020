using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day22
{
    public class Day22Solver : SolverBase
    {
        public override string DayName => "Crab Combat";

        private static (Queue<int> player1Deck, Queue<int> player2Deck) ParseInputToDecks(string input)
        {
            IReadOnlyList<IReadOnlyList<int>> originalDecks = input
                .NormalizeLineEndings()
                .Split($"{Environment.NewLine}{Environment.NewLine}")
                .Select(deck => deck.ReadLines().ToArray()[1..].Select(int.Parse).ToArray())
                .ToArray();

            var player1Deck = new Queue<int>(originalDecks[0]);
            var player2Deck = new Queue<int>(originalDecks[1]);

            return (player1Deck, player2Deck);
        }

        private static void EndOfRound(bool player1Won, Queue<int> player1Deck, int player1Card, Queue<int> player2Deck, int player2Card)
        {
            if (player1Won)
            {
                player1Deck.Enqueue(player1Card);
                player1Deck.Enqueue(player2Card);
            }
            else
            {
                player2Deck.Enqueue(player2Card);
                player2Deck.Enqueue(player1Card);
            }
        }

        private static long CalculateScore(IEnumerable<int> winningDeck) =>
            winningDeck.Reverse().Select((card, i) => (card, n: i + 1)).Aggregate(0L, (agg, cur) => agg + cur.card * cur.n);

        protected override long? SolvePart1Impl(string input)
        {
            var (player1Deck, player2Deck) = ParseInputToDecks(input);

            var roundNumber = 0;
            while (player1Deck.Any() && player2Deck.Any())
            {
                roundNumber++;
                var player1Card = player1Deck.Dequeue();
                var player2Card = player2Deck.Dequeue();

                if (player1Card == player2Card)
                {
                    throw new InvalidOperationException($"Draw not supported! {new {roundNumber, player1Card, player2Card}}");
                }

                var player1Won = player1Card > player2Card;

                EndOfRound(player1Won, player1Deck, player1Card, player2Deck, player2Card);
            }

            var winningDeck = player1Deck.Any() ? player1Deck : player2Deck;

            return CalculateScore(winningDeck);
        }

        protected override long? SolvePart2Impl(string input)
        {
            var (player1Deck, player2Deck) = ParseInputToDecks(input);

            var (_, winningDeck) = Part2Game(player1Deck, player2Deck);

            return CalculateScore(winningDeck);
        }

        private static (bool player1WonGame, IEnumerable<int> winningDeck) Part2Game(
            Queue<int> player1Deck,
            Queue<int> player2Deck)
        {
            var roundNumber = 0;
            var gameHistory = new HashSet<string>();
            var infiniteGameDetected = false;
            while (player1Deck.Any() && player2Deck.Any())
            {
                roundNumber++;

                var gameId = $"{string.Join(",", player1Deck)}|{string.Join(",", player2Deck)}";
                if (!gameHistory.Add(gameId))
                {
                    infiniteGameDetected = true;
                    break;
                }

                var player1Card = player1Deck.Dequeue();
                var player2Card = player2Deck.Dequeue();

                if (player1Card == player2Card)
                {
                    throw new InvalidOperationException($"Draw not supported! {new {roundNumber, player1Card, player2Card}}");
                }

                var recursiveCombatRequired = player1Deck.Count >= player1Card &&
                                              player2Deck.Count >= player2Card;

                var player1Won = recursiveCombatRequired
                    ? RecursiveCombat(player1Deck, player1Card, player2Deck, player2Card).player1WonSubGame
                    : player1Card > player2Card;

                EndOfRound(player1Won, player1Deck, player1Card, player2Deck, player2Card);
            }

            var player1WonGame = infiniteGameDetected || player1Deck.Any();

            return (player1WonGame, player1WonGame ? player1Deck : player2Deck);
        }

        private static (bool player1WonSubGame, IEnumerable<int> winningDeck) RecursiveCombat(
            IEnumerable<int> player1Deck,
            int player1Card,
            IEnumerable<int> player2Deck,
            int player2Card)
        {
            var player1NewDeck = new Queue<int>(player1Deck.Take(player1Card));
            var player2NewDeck = new Queue<int>(player2Deck.Take(player2Card));
            return Part2Game(player1NewDeck, player2NewDeck);
        }
    }
}
