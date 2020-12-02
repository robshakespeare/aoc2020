using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day2
{
    public class PasswordLine
    {
        public int LowerBound { get; set; }

        public int UpperBound { get; set; }

        public char RequiredChar { get; set; }

        public string Password { get; set; } = "";

        private static readonly Regex ParserRegex = new Regex(
            @"(?<LowerBound>\d+)-(?<UpperBound>\d+) (?<RequiredChar>\w): (?<Password>.+)",
            RegexOptions.Compiled);

        public static PasswordLine Parse(string line)
        {
            var match = ParserRegex.Match(line);

            if (!match.Success)
            {
                throw new InvalidOperationException("Invalid password line:" + line);
            }

            return new PasswordLine
            {
                LowerBound = int.Parse(match.Groups["LowerBound"].Value),
                UpperBound = int.Parse(match.Groups["UpperBound"].Value),
                RequiredChar = match.Groups["RequiredChar"].Value.Single(),
                Password = match.Groups["Password"].Value
            };
        }
    }
}
