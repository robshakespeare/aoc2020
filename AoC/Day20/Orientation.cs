using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC.Day20
{
    public record Orientation(Rotation Rotation, Scale Scale)
    {
        public static readonly IReadOnlyList<Orientation> Permutations =
            Enum.GetValues(typeof(Rotation)).Cast<Rotation>()
                .SelectMany(rotation => Enum.GetValues(typeof(Scale)).Cast<Scale>().Select(scale => new Orientation(rotation, scale)))
                .ToArray();

        public IReadOnlyList<string> Apply(IReadOnlyList<string> pixels)
        {
            var (rotation, scale) = this;

            if (!(rotation is Rotation.Zero or Rotation.Right90 or Rotation.Right180 or Rotation.Right270))
            {
                throw new InvalidOperationException("Invalid rotation: " + rotation);
            }

            var rotated = MathUtils.RotateGrid(pixels, 90 * (int)rotation);

            var scaleBy = scale switch
            {
                Scale.None => new Vector2(1, 1),
                Scale.FlipHorizontal => new Vector2(1, -1),
                Scale.FlipVertical => new Vector2(-1, 1),
                _ => throw new InvalidOperationException("Invalid scale: " + scale)
            };

            return MathUtils.ScaleGrid(rotated, scaleBy);
        }
    }

    public enum Rotation
    {
        Zero = 0,
        Right90 = 1,
        Right180 = 2,
        Right270 = 3
    }

    public enum Scale
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVertical = 2
    }
}
