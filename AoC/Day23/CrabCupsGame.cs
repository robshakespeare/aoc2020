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
        private LinkedListNode<int> _currentCup;
        private bool _hasPickedCups;
        private PickedCups _pickedCups;

        public CrabCupsGame(string puzzleInput)
        {
            var cupValues = puzzleInput
                .Select(chr => int.Parse(chr.ToString()))
                .ToArray();

            _cupsList = new LinkedList<int>(cupValues);
            _minCup = _cupsList.Min();
            _maxCup = _cupsList.Max();
            _currentCup = _cupsList.First ?? throw new InvalidOperationException("Could not get first cup, is puzzle input empty?");
        }

        public void Play(int numOfMoves)
        {
            for (var moveNum = 1; moveNum <= numOfMoves; moveNum++)
            {
                // Pick up cups
                _hasPickedCups = true;
                _pickedCups.Picked1 = PickUpNext();
                _pickedCups.Picked2 = PickUpNext();
                _pickedCups.Picked3 = PickUpNext();

                // Assign next current cup
                var destinationCup = GetDestinationCup();

                // Place cups
                _cupsList.AddAfter(destinationCup, _pickedCups.Picked3);
                _cupsList.AddAfter(destinationCup, _pickedCups.Picked2);
                _cupsList.AddAfter(destinationCup, _pickedCups.Picked1);
                _hasPickedCups = false;

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
                    found = _cupsList.Find(searchFor) ?? throw new InvalidOperationException("Unable to find cup with value: " + searchFor);
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

        private LinkedListNode<int> GetNextOrFirst(LinkedListNode<int> node) => node.Next ?? node.List?.First ?? throw new InvalidOperationException();

        private bool IsPickedCup(int cupValue) =>
            _hasPickedCups &&
            _pickedCups.Picked1.Value == cupValue ||
            _pickedCups.Picked2.Value == cupValue ||
            _pickedCups.Picked3.Value == cupValue;

        private struct PickedCups
        {
            public LinkedListNode<int> Picked1;
            public LinkedListNode<int> Picked2;
            public LinkedListNode<int> Picked3;
        }
    }
}
