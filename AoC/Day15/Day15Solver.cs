using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AoC.Day15
{
    public class Day15Solver : SolverBase
    {
        public override string DayName => "Rambunctious Recitation";

        protected override long? SolvePart1Impl(string input) => PlayTillTurn(input, 2020);

        protected override long? SolvePart2Impl(string input) => PlayTillTurn(input, 30000000);

        private static long? PlayTillTurn(string input, int playToTurnNumber)
        {
            var memory = Memory.SeedWith(input);

            while (memory.NextTurnNumber <= playToTurnNumber)
            {
                if (memory.HasLastNumberBeenSpokenBefore(out var turnsItWasSpoken))
                {
                    // The last number had been spoken before that, so get the delta between the last 2 times it was spoken and speak that as the new number.
                    var delta = turnsItWasSpoken[^1] - turnsItWasSpoken[^2];
                    memory.RecordNumber(delta);
                }
                else
                {
                    // This is the first time the last number had been spoken, so the new spoken number is 0.
                    memory.RecordNumber(0);
                }
            }

            return memory.LastNumberSpoken;
        }

        public class Memory
        {
            private readonly Dictionary<int, List<int>> _numberToTurnsMap = new(); // Key is the Number; Value is the Turns that Number has been spoken.
            private readonly List<int> _numbersSpoken = new();

            public int LastNumberSpoken { get; private set; } = -1;

            public int NextTurnNumber { get; private set; } = 1;

            public IReadOnlyList<int> NumbersSpoken => _numbersSpoken;

            public Memory(IEnumerable<int> seedNumbers) => seedNumbers.ForEach(RecordNumber);

            public void RecordNumber(int number)
            {
                var turn = NextTurnNumber;

                if (!_numberToTurnsMap.TryGetValue(number, out var listOfTurnsItWasSpoken))
                {
                    _numberToTurnsMap[number] = listOfTurnsItWasSpoken = new List<int>();
                }

                listOfTurnsItWasSpoken.Add(turn);
                LastNumberSpoken = number;
                _numbersSpoken.Add(number);
                NextTurnNumber++;
            }

            public bool HasLastNumberBeenSpokenBefore(out IReadOnlyList<int> turnsItWasSpoken)
            {
                if (_numberToTurnsMap.TryGetValue(LastNumberSpoken, out var listOfTurnsItWasSpoken))
                {
                    turnsItWasSpoken = listOfTurnsItWasSpoken;
                    return turnsItWasSpoken.Count > 1;
                }

                turnsItWasSpoken = Array.Empty<int>();
                return false;
            }

            public static Memory SeedWith(string input) => new(input.Split(",").Select(int.Parse));
        }
    }
}
