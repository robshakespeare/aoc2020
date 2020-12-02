using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day2
{
    public class PasswordLinePolicy2
    {
        /// <summary>
        /// Position 1, one-based index.
        /// </summary>
        public int Position1 { get; set; }

        /// <summary>
        /// Position 2, one-based index.
        /// </summary>
        public int Position2 { get; set; }

        public char RequiredChar { get; set; }

        public string Password { get; set; } = "";

        public bool IsValid
        {
            get
            {
                var index1 = Position1 - 1;
                var index2 = Position2 - 1;

                var chars = new[] { Password[index1], Password[index2] };

                return chars.Count(c => c == RequiredChar) == 1;
            }
        }

        private static readonly Regex ParserRegex = new Regex(
            @"(?<Position1>\d+)-(?<Position2>\d+) (?<RequiredChar>\w): (?<Password>.+)",
            RegexOptions.Compiled);

        public static PasswordLinePolicy2 Parse(string line)
        {
            var match = ParserRegex.Match(line);

            if (!match.Success)
            {
                throw new InvalidOperationException("Invalid password line:" + line);
            }

            return new PasswordLinePolicy2
            {
                Position1 = int.Parse(match.Groups["Position1"].Value),
                Position2 = int.Parse(match.Groups["Position2"].Value),
                RequiredChar = match.Groups["RequiredChar"].Value.First(),
                Password = match.Groups["Password"].Value
            };
        }
    }
}
