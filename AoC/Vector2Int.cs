using System;
using System.Numerics;

namespace AoC
{
    public readonly struct Vector2Int
    {
        public int X { get; }

        public int Y { get; }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static readonly Vector2Int Zero = new();

        public int ManhattanDistanceFromZero => ManhattanDistance(this, Zero);

        public static int ManhattanDistance(Vector2Int a, Vector2Int b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);

        public static Vector2Int operator /(Vector2Int a, int b) => new(a.X / b, a.Y / b);

        public static Vector2Int FromVector2(Vector2 vector) => new((int) Math.Round(vector.X), (int) Math.Round(vector.Y));

        public override string ToString() => $"{X},{Y}";
    }
}
