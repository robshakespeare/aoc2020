using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Day23
{
    public class CrabCupsGame
    {
        private readonly LinkedList<int> _cupsList;
        private readonly int _minCup;
        private readonly int _maxCup;
        private readonly IReadOnlyList<LinkedListNode<int>> _cupsLookup;

        private LinkedListNode<int> _currentCup;
        private LinkedListNode<int> _pickedCup1;
        private LinkedListNode<int> _pickedCup2;
        private LinkedListNode<int> _pickedCup3;

        public CrabCupsGame(string puzzleInput, bool isPart2)
        {
            var cupValues = puzzleInput
                .Select(chr => int.Parse(chr.ToString()))
                .ToArray();

            if (isPart2)
            {
                var extraCount = 1000000 - cupValues.Length;

                cupValues = cupValues.Concat(Enumerable.Range(cupValues.Max() + 1, extraCount)).ToArray();
            }

            _cupsList = new LinkedList<int>(cupValues);
            _minCup = _cupsList.Min();
            _maxCup = _cupsList.Max();
            _cupsLookup = EnumerateNodes(_cupsList.First).OrderBy(cupNode => cupNode.Value).ToArray();

            _currentCup = _cupsList.First ?? throw new InvalidOperationException("Could not get first cup, is puzzle input empty?");
            _pickedCup1 = new LinkedListNode<int>(-1);
            _pickedCup2 = new LinkedListNode<int>(-1);
            _pickedCup3 = new LinkedListNode<int>(-1);

            static IEnumerable<LinkedListNode<int>> EnumerateNodes(LinkedListNode<int>? current)
            {
                while (current != null)
                {
                    yield return current;
                    current = current.Next;
                }
            }
        }

        private static LinkedListNode<int> GetNextOrFirst(LinkedListNode<int> node) => node.Next ?? node.List?.First ?? throw new InvalidOperationException();

        /// <summary>
        /// Simulates the specified next number of moves.
        /// </summary>
        public void Play(int numOfMoves)
        {
            for (var moveNum = 1; moveNum <= numOfMoves; moveNum++)
            {
                // Pick up cups
                _pickedCup1 = PickUpNext();
                _pickedCup2 = PickUpNext();
                _pickedCup3 = PickUpNext();

                // Assign next current cup
                var destinationCup = GetDestinationCup();

                // Place cups
                _cupsList.AddAfter(destinationCup, _pickedCup3);
                _cupsList.AddAfter(destinationCup, _pickedCup2);
                _cupsList.AddAfter(destinationCup, _pickedCup1);

                // Select new current cup
                _currentCup = GetNextOrFirst(_currentCup);
            }
        }

        private LinkedListNode<int> PickUpNext()
        {
            var next = GetNextOrFirst(_currentCup);
            _cupsList.Remove(next);
            return next;
        }

        private bool IsPickedCup(int cupValue) =>
            _pickedCup1.Value == cupValue ||
            _pickedCup2.Value == cupValue ||
            _pickedCup3.Value == cupValue;

        private LinkedListNode<int> GetDestinationCup()
        {
            var destinationCupValue = _currentCup.Value - 1;
            LinkedListNode<int>? found = null;

            do
            {
                if (destinationCupValue < _minCup)
                {
                    destinationCupValue = _maxCup;
                }

                if (!IsPickedCup(destinationCupValue))
                {
                    found = LookupCup(destinationCupValue);
                }
                else
                {
                    destinationCupValue--;
                }
            } while (found == null);

            return found;
        }

        private LinkedListNode<int> LookupCup(int cupValue)
        {
            var cupIndex = cupValue - 1;
            return _cupsLookup[cupIndex];
        }

        /// <summary>
        /// After the crab is done, what order will the cups be in?
        /// Starting after the cup labeled 1, collect the other cups' labels clockwise into a single
        /// string with no extra characters; each number except 1 should appear exactly once.
        /// </summary>
        public long GetCupOrder()
        {
            var result = new StringBuilder();

            var current = _cupsList.Find(1) ?? throw new InvalidOperationException("Did not find cup 1 in cup list");

            do
            {
                if (current.Value != 1)
                {
                    result.Append(current.Value);
                }

                current = GetNextOrFirst(current);
            } while (current.Value != 1);

            return long.Parse(result.ToString());
        }

        /// <summary>
        /// Determine which two cups will end up immediately clockwise of cup 1.
        /// What do you get if you multiply their labels together?
        /// </summary>
        public long GetPart2Result()
        {
            var cup1 = LookupCup(1);

            var nextNode1 = GetNextOrFirst(cup1);
            var nextNode2 = GetNextOrFirst(nextNode1);

            return nextNode1.Value * (long) nextNode2.Value;
        }
    }
}
