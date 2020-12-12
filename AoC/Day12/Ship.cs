using System;
using System.Linq;
using System.Numerics;

namespace AoC.Day12
{
    public class Ship
    {
        public static readonly Vector2 North = new(0, -1);
        public static readonly Vector2 East = new(1, 0);
        public static readonly Vector2 South = new(0, 1);
        public static readonly Vector2 West = new(-1, 0);

        private Vector2 _direction = East;

        public Vector2 Position { get; protected set; } = Vector2.Zero;

        /// <summary>
        /// Processes the navigation instructions and then returns the Manhattan distance between that ship's ending position and starting position.
        /// </summary>
        public int Navigate(string input)
        {
            foreach (var (action, inputValue) in input.ReadLines()
                .Select(line => (line[0], int.Parse(line[1..]))))
            {
                ProcessInstruction(action, inputValue);
            }

            return Vector2Int.FromVector2(Position).ManhattanDistanceFromZero;
        }

        protected virtual void ProcessInstruction(char action, int inputValue)
        {
            Position = action switch
            {
                'N' => Move(North, inputValue),
                'S' => Move(South, inputValue),
                'E' => Move(East, inputValue),
                'W' => Move(West, inputValue),
                'L' => Turn(-inputValue),
                'R' => Turn(inputValue),
                'F' => MoveForward(inputValue),
                _ => throw new InvalidOperationException("Invalid action " + action)
            } ?? Position;
        }

        private Vector2? Move(Vector2 direction, int amount) => Position + direction * amount;

        private Vector2? MoveForward(int amount) => Move(_direction, amount);

        private Vector2? Turn(int degrees)
        {
            _direction = RotateDirection(_direction, degrees);
            return null;
        }

        private const double DegreesToRadians = Math.PI / 180;

        public static Vector2 RotateDirection(Vector2 direction, int degrees)
        {
            var radians = Convert.ToSingle(degrees * DegreesToRadians);

            var rotationMatrix = Matrix3x2.CreateRotation(radians);

            return Vector2.Transform(direction, rotationMatrix);
        }
    }
}
