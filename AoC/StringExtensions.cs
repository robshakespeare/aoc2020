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
        /// Parses and returns each line in the input string. Trims any trailing line endings.
        /// </summary>
        public static string[] ReadAllLines(this string s)
        {
            return ReadAllLinesEnumerable(s ?? throw new ArgumentNullException(nameof(s))).ToArray();

            static IEnumerable<string> ReadAllLinesEnumerable(string s)
            {
                using var sr = new StringReader(s.NormalizeLineEndings().TrimEnd(Environment.NewLine.ToCharArray()));
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        private static readonly Regex LineEndingsRegex = new Regex(@"\r\n|\n|\r", RegexOptions.Compiled);

        /// <summary>
        /// Normalizes the line endings in the specified string, so that all the line endings match the current environment's line endings.
        /// </summary>
        public static string NormalizeLineEndings(this string s) =>
            LineEndingsRegex.Replace(s ?? throw new ArgumentNullException(nameof(s)), Environment.NewLine);
    }
}
