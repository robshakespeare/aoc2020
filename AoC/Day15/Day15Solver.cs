using System.Linq;
using MoreLinq;

namespace AoC.Day15
{
    public class Day15Solver : SolverBase
    {
        public override string DayName => "Rambunctious Recitation";

        protected override long? SolvePart1Impl(string input) => new MemoryGame(input, 2020).Play();

        protected override long? SolvePart2Impl(string input) => new MemoryGame(input, 30000000).Play();

        public class MemoryGame
        {
            private readonly int _playToTurnNumber;
            private readonly int[] _numberToTurnMap; // Index is the Number; Element is the Turn that number has last been spoken.

            public MemoryGame(string input, int playToTurnNumber)
            {
                _playToTurnNumber = playToTurnNumber;
                _numberToTurnMap = new int[playToTurnNumber];
                input.Split(",").Select(int.Parse).ForEach(Update);
            }

            public int LastNumberSpoken { get; private set; } = -1;

            public int NextNumberToSpeak { get; private set; } = -1;

            public int TurnCounter { get; private set; } = 1;

            public int Play()
            {
                while (TurnCounter <= _playToTurnNumber)
                {
                    Update(NextNumberToSpeak);
                }

                return LastNumberSpoken;
            }

            private void Update(int number)
            {
                var turn = TurnCounter;

                var prevTurnItWasSpoken = _numberToTurnMap[number];
                var wasPreviouslySpoken = prevTurnItWasSpoken != 0;

                _numberToTurnMap[number] = turn;

                LastNumberSpoken = number;

                // If the last number had been spoken before that, get the delta between the last 2 times it was spoken and speak that as the next number.
                // Else, this is the first time the last number had been spoken, so the new spoken number is 0.
                NextNumberToSpeak = wasPreviouslySpoken ? turn - prevTurnItWasSpoken : 0;

                TurnCounter++;
            }
        }
    }
}
