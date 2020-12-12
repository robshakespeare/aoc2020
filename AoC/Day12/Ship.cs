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

        public Vector2 Direction { get; private set; } = East;

        public Vector2 Position { get; private set; } = Vector2.Zero;

        /// <summary>
        /// Processes the navigation instructions and then returns the Manhattan distance between that ship's ending position and starting position.
        /// </summary>
        public int Navigate(string input)
        {
            foreach (var instruction in input.ReadLines()
                .Select(line => new { action = line[0], inputValue = int.Parse(line[1..]) }))
            {
                Position = instruction.action switch
                {
                    'N' => Move(North, instruction.inputValue),
                    'S' => Move(South, instruction.inputValue),
                    'E' => Move(East, instruction.inputValue),
                    'W' => Move(West, instruction.inputValue),
                    'L' => Turn(-instruction.inputValue),
                    'R' => Turn(instruction.inputValue),
                    'F' => MoveForward(instruction.inputValue),
                    _ => throw new InvalidOperationException("Invalid action " + instruction.action)
                };
            }

            return Vector2Int.FromVector2(Position).ManhattanDistanceFromZero;
        }

        private Vector2 Move(Vector2 direction, int amount) => Position + direction * amount;

        private Vector2 MoveForward(int amount) => Move(Direction, amount);

        private Vector2 Turn(int degrees)
        {
            Direction = RotateDirection(Direction, degrees);
            return Position;
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
