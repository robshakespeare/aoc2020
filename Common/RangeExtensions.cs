using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class RangeExtensions
    {
        /// <summary>
        /// Returns the indexes in specified Range, from the inclusive start index of the Range to the exclusive end index of the Range.
        /// </summary>
        public static int[] ToArray(this Range range) => ToEnumerable(range).ToArray();

        /// <summary>
        /// Enumerates the indexes in specified Range, from the inclusive start index of the Range to the exclusive end index of the Range.
        /// </summary>
        public static IEnumerable<int> ToEnumerable(this Range range)
        {
            for (var i = range.Start.Value; i < range.End.Value; i++)
            {
                yield return i;
            }
        }
    }
}
