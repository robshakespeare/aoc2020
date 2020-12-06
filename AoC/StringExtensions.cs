using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC
{
    public static class StringExtensions
    {
        /// <summary>
        /// Parses and returns each line in the input string. 
        /// </summary>
        public static IEnumerable<string> ReadLines(this string s)
        {
            using var sr = new StringReader(s ?? throw new ArgumentNullException(nameof(s)));
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                yield return line;
            }
        }

        /// <summary>
        /// Parses and returns each line in the input string as an Int64.
        /// </summary>
        public static IEnumerable<long> ReadLinesAsLongs(this string s) => s.ReadLines().Select(long.Parse);

        private static readonly Regex LineEndingsRegex = new(@"\r\n|\n|\r", RegexOptions.Compiled);

        /// <summary>
        /// Normalizes the line endings in the specified string, so that all the line endings match the current environment's line endings.
        /// </summary>
        public static string NormalizeLineEndings(this string? s) => LineEndingsRegex.Replace(s ?? "", Environment.NewLine);

        /// <summary>
        /// Normalizes the line endings in the specified string, and trims any trailing line endings.
        /// </summary>
        public static string NormalizeAndTrimEnd(this string? s) => s.NormalizeLineEndings().TrimEnd(Environment.NewLine.ToCharArray());
    }
}
