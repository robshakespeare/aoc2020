using System;
using System.Numerics;

namespace AoC
{
    public static class MathUtils
    {
        /// <summary>
        /// Finds the greatest common divisor (GCD) of the two specified numbers.
        /// </summary>
        public static long GreatestCommonDivisor(long a, long b) => (long)BigInteger.GreatestCommonDivisor(a, b);

        /// <summary>
        /// Returns the least common multiple (LCM) of the two specified numbers.
        /// </summary>
        public static long LeastCommonMultiple(long a, long b) => a * b / GreatestCommonDivisor(a, b);

        /// <summary>
        /// Returns the least common multiple (LCM) of the three specified numbers.
        /// </summary>
        public static long LeastCommonMultiple(long a, long b, long c) => LeastCommonMultiple(LeastCommonMultiple(a, b), c);

        /// <summary>
        /// Calculates the angle, in degrees, between the two specified vectors.
        /// </summary>
        public static double AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            var sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            var cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            var angleBetween = Math.Atan2(sin, cos) * (180 / Math.PI);
            return angleBetween < 0 ? 360 + angleBetween : angleBetween;
        }

        public static readonly Vector2 North = new(0, -1);
        public static readonly Vector2 East = new(1, 0);
        public static readonly Vector2 South = new(0, 1);
        public static readonly Vector2 West = new(-1, 0);

        private const double DegreesToRadians = Math.PI / 180;

        /// <summary>
        /// Rotates the specified direction vector, around the zero vector (0,0) center point, by the specified number of degrees.
        /// </summary>
        /// <remarks>
        /// Note: tis method expects direction vector to be centered around the zero vector (0,0).
        /// It could probably be extended, using the Matrix3x2 CreateRotation(float radians, Vector2 centerPoint) method,
        /// to cater for rotation around a non-zero center point.
        /// </remarks>
        public static Vector2 RotateDirection(Vector2 direction, int degrees)
        {
            var radians = Convert.ToSingle(degrees * DegreesToRadians);

            var rotationMatrix = Matrix3x2.CreateRotation(radians);

            return Vector2.Transform(direction, rotationMatrix);
        }
    }
}
