using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day16
{
    public record FieldRule(string FieldName, IReadOnlyList<Range> Ranges)
    {
        /// <summary>
        /// The value is valid if it falls within any of the ranges.
        /// </summary>
        public bool IsValidValue(int value) => Ranges.Any(range => value >= range.Start.Value && value <= range.End.Value);

        public static FieldRule[] ParseRules(string rules) => rules.ReadLines().Select(ParseRule).ToArray();

        private static readonly Regex ParseRuleRegex = new(@"(?<FieldName>[^:]+): ((?<Range>\d+-\d+)|( or ))+", RegexOptions.Compiled);

        public static FieldRule ParseRule(string rule)
        {
            var match = ParseRuleRegex.Match(rule);
            if (!match.Success)
            {
                throw new InvalidOperationException("Rule not valid: " + rule);
            }

            return new FieldRule(
                match.Groups["FieldName"].Value,
                match.Groups["Range"].Captures.Select(c =>
                {
                    var rangeParts = c.Value.Split("-");
                    return new Range(int.Parse(rangeParts[0]), int.Parse(rangeParts[1]));
                }).ToArray());
        }
    }
}
