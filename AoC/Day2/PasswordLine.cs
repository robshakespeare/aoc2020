using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day2
{
    public record PasswordLine(
        int LowerBound,
        int UpperBound,
        char RequiredChar,
        string Password)
    {
        private static readonly Regex ParserRegex = new(
            @"^(?<LowerBound>\d+)-(?<UpperBound>\d+) +(?<RequiredChar>.): +(?<Password>.+)$",
            RegexOptions.Compiled);

        public static PasswordLine Parse(string line)
        {
            var match = ParserRegex.Match(line ?? throw new ArgumentNullException(nameof(line)));

            return !match.Success
                ? throw new InvalidOperationException("Invalid password line: " + line)
                : new PasswordLine(
                    int.Parse(match.Groups["LowerBound"].Value),
                    int.Parse(match.Groups["UpperBound"].Value),
                    match.Groups["RequiredChar"].Value.Single(),
                    match.Groups["Password"].Value);
        }
    }
}
