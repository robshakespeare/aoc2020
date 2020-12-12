using System;
using System.Numerics;
using static AoC.MathUtils;

namespace AoC.Day12
{
    public class Ship2 : Ship
    {
        public Vector2 Waypoint { get; protected set; } = new(10, -1);

        protected override void ProcessInstruction(char action, int inputValue)
        {
            Position = action switch
            {
                'N' => MoveWaypoint(North, inputValue),
                'S' => MoveWaypoint(South, inputValue),
                'E' => MoveWaypoint(East, inputValue),
                'W' => MoveWaypoint(West, inputValue),
                'L' => TurnWaypoint(-inputValue),
                'R' => TurnWaypoint(inputValue),
                'F' => MoveToWaypoint(inputValue),
                _ => throw new InvalidOperationException("Invalid action " + action)
            } ?? Position;
        }

        private Vector2? MoveWaypoint(Vector2 direction, int amount)
        {
            Waypoint += direction * amount;
            return null;
        }

        private Vector2? MoveToWaypoint(int amount) => Position + Waypoint * amount;

        private Vector2? TurnWaypoint(int degrees)
        {
            Waypoint = RotateDirection(Waypoint, degrees);
            return null;
        }
    }
}
