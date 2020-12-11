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
    }
}
