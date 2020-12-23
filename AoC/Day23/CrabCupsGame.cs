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

        public CrabCupsGame(string puzzleInput)
        {
            var cupValues = puzzleInput
                .Select(chr => int.Parse(chr.ToString()))
                .ToArray();

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

        private LinkedListNode<int> GetDestinationCup()
        {
            var searchFor = _currentCup.Value - 1;
            LinkedListNode<int>? found = null;
                
            do
            {
                if (searchFor < _minCup)
                {
                    searchFor = _maxCup;
                }

                if (!IsPickedCup(searchFor))
                {
                    var cupIndex = searchFor - 1;
                    found = _cupsLookup[cupIndex];
                }
                else
                {
                    searchFor--;
                }
            } while (found == null);

            return found;
        }

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

        private LinkedListNode<int> PickUpNext()
        {
            var next = GetNextOrFirst(_currentCup);
            _cupsList.Remove(next);
            return next;
        }

        private static LinkedListNode<int> GetNextOrFirst(LinkedListNode<int> node) => node.Next ?? node.List?.First ?? throw new InvalidOperationException();

        private bool IsPickedCup(int cupValue) =>
            _pickedCup1.Value == cupValue ||
            _pickedCup2.Value == cupValue ||
            _pickedCup3.Value == cupValue;

        private struct PickedCups
        {
            
        }
    }
}
