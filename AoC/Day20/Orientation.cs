using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day20
{
    public record Orientation(Rotation Rotation, Scale Scale)
    {
        public static readonly IReadOnlyList<Orientation> Permutations =
            Enum.GetValues(typeof(Rotation)).Cast<Rotation>()
                .SelectMany(rotation => Enum.GetValues(typeof(Scale)).Cast<Scale>().Select(scale => new Orientation(rotation, scale)))
                .ToArray();
    }
}
