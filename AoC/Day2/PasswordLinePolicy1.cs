using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day2
{
    public class PasswordLinePolicy1
    {
        /// <summary>
        /// Lower bound, inclusive.
        /// </summary>
        public int LowerBound { get; set; }

        /// <summary>
        /// Upper bound, inclusive.
        /// </summary>
        public int UpperBound { get; set; }

        public char RequiredChar { get; set; }

        public string Password { get; set; } = "";

        public bool IsValid
        {
            get
            {
                var requireCharCount = Password.Count(c => c == RequiredChar);
                return requireCharCount >= LowerBound && requireCharCount <= UpperBound;
            }
        }

        private static readonly Regex ParserRegex = new Regex(
            @"(?<LowerBound>\d+)-(?<UpperBound>\d+) (?<RequiredChar>\w): (?<Password>.+)",
            RegexOptions.Compiled);

        public static PasswordLinePolicy1 Parse(string line)
        {
            var match = ParserRegex.Match(line);

            if (!match.Success)
            {
                throw new InvalidOperationException("Invalid password line:" + line);
            }

            return new PasswordLinePolicy1
            {
                LowerBound = int.Parse(match.Groups["LowerBound"].Value),
                UpperBound = int.Parse(match.Groups["UpperBound"].Value),
                RequiredChar = match.Groups["RequiredChar"].Value.First(),
                Password = match.Groups["Password"].Value
            };
        }
    }
}
