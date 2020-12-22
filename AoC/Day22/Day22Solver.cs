using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day22
{
    public class Day22Solver : SolverBase
    {
        public override string DayName => "Crab Combat";

        protected override long? SolvePart1Impl(string input)
        {
            IReadOnlyList<IReadOnlyList<int>> originalDecks = input
                .NormalizeLineEndings()
                .Split($"{Environment.NewLine}{Environment.NewLine}")
                .Select(deck => deck.ReadLines().ToArray()[1..].Select(int.Parse).ToArray())
                .ToArray();

            var player1Deck = new Queue<int>(originalDecks[0]);
            var player2Deck = new Queue<int>(originalDecks[1]);

            if (player1Deck.Count != player2Deck.Count)
            {
                throw new InvalidOperationException("Player deck sizes do not match.");
            }

            var roundNumber = 0;
            while (player1Deck.Any() && player2Deck.Any())
            {
                roundNumber++;
                var player1Card = player1Deck.Dequeue();
                var player2Card = player2Deck.Dequeue();

                if (player1Card == player2Card)
                {
                    throw new InvalidOperationException($"Draw not supported! {new{ roundNumber, player1Card, player2Card }}");
                }

                var player1Won = player1Card > player2Card;

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

            var winningDeck = player1Deck.Any() ? player1Deck : player2Deck;

            return winningDeck.Reverse().Select((card, i) => (card, n: i + 1)).Aggregate(0L, (agg, cur) => agg + cur.card * cur.n);
        }

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
    }
}
