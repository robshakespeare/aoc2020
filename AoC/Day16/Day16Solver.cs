using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Environment;

namespace AoC.Day16
{
    public class Day16Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input) => TrainServiceNotes.Parse(input).GetInvalidTicketValues().Sum();

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }

        public record TrainServiceNotes(
            IReadOnlyList<FieldRule> FieldRules,
            Ticket YourTicket,
            IReadOnlyList<Ticket> OtherTickets)
        {
            public static TrainServiceNotes Parse(string input)
            {
                var parts = input.NormalizeLineEndings().Split(NewLine + NewLine);

                if (!parts[1].StartsWith("your ticket:") || !parts[2].StartsWith("nearby tickets:"))
                {
                    throw new InvalidOperationException("Ticket not valid: " + input);
                }

                return new TrainServiceNotes(
                    FieldRule.ParseRules(parts[0]),
                    Ticket.ParseTicket(parts[1].ReadLines().Skip(1).Single()),
                    parts[2].ReadLines().Skip(1).Select(Ticket.ParseTicket).ToArray());
            }

            public IEnumerable<int> GetInvalidTicketValues()
            {
                return OtherTickets.SelectMany(ticket => ticket.GetInvalidValues(FieldRules));
            }
        }

        public record FieldRule(
            string FieldName,
            IReadOnlyList<Range> Ranges)
        {
            private static readonly Regex ParseRuleRegex = new(@"(?<FieldName>\w+): ((?<Range>\d+-\d+)|( or ))+", RegexOptions.Compiled);

            public static FieldRule[] ParseRules(string rules) => rules.ReadLines().Select(ParseRule).ToArray();

            /// <summary>
            /// The value is valid if it falls within any of the ranges.
            /// </summary>
            public bool IsValidValue(int value) => Ranges.Any(range => value >= range.Start.Value && value <= range.End.Value);

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

        public record Ticket(
            IReadOnlyList<int> Values)
        {
            public static Ticket ParseTicket(string ticket) => new(ticket.Split(",").Select(int.Parse).ToArray());

            /// <summary>
            /// Each value is invalid if it does not match a single rule.
            /// </summary>
            public IEnumerable<int> GetInvalidValues(IEnumerable<FieldRule> rules) => Values.Where(value => !rules.Any(rule => rule.IsValidValue(value)));
        }
    }
}
