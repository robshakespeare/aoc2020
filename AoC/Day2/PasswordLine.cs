using System;
using Sprache;
using static Sprache.Parse;

namespace AoC.Day2
{
    public record PasswordLine(
        int LowerBound,
        int UpperBound,
        char RequiredChar,
        string Password)
    {
        private static readonly Parser<PasswordLine> Parser =
            from lowerBound in Number.Select(int.Parse)
            from _ in Char('-')
            from upperBound in Number.Select(int.Parse)
            from __ in Char(' ')
            from requiredChar in AnyChar
            from ___ in String(": ")
            from password in AnyChar.AtLeastOnce().Text()
            select new PasswordLine(lowerBound, upperBound, requiredChar, password);

        public static PasswordLine Parse(string line)
        {
            if (line == null) throw new ArgumentNullException(nameof(line));
            try
            {
                return Parser.Parse(line);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Invalid password line: " + line, e);
            }
        }
    }
}
