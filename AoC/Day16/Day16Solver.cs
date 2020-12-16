using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Environment;

namespace AoC.Day16
{
    public class Day16Solver : SolverBase
    {
        public override string DayName => "Ticket Translation";

        protected override long? SolvePart1Impl(string input) => TrainServiceNotes.Parse(input).GetInvalidTicketValues().Sum();

        protected override long? SolvePart2Impl(string input) => TrainServiceNotes.Parse(input).DetermineOrderOfFields();

        public record TrainServiceNotes(
            IReadOnlyList<FieldRule> Rules,
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
                return OtherTickets.SelectMany(ticket => ticket.GetInvalidValues(Rules));
            }

            public IEnumerable<Ticket> GetValidTickets() => new[] { YourTicket }.Concat(OtherTickets.Where(ticket => ticket.IsValid(Rules)));

            public long DetermineOrderOfFields()
            {
                var numFields = YourTicket.Values.Count;
                if (Rules.Count != numFields)
                {
                    throw new InvalidOperationException($"Number of rules {Rules.Count} does not match num of fields {numFields}.");
                }

                var validTickets = GetValidTickets().ToArray();

                // For each field, get the subset of rules which match
                var candidates = Enumerable.Range(0, numFields)
                    .Select(fieldIndex => GetCandidateRules(fieldIndex, validTickets).ToList())
                    .ToReadOnlyArray();

                // Use process of elimination, to whittle each subset down to just one rule, once reached, we know which field each rule refers to
                while (candidates.Any(candidate => candidate.Count > 1))
                {
                    // Eliminate any whose count is 1
                    foreach (var knownField in candidates.Where(x => x.Count == 1))
                    {
                        var knownFieldName = knownField.Single().FieldName;

                        foreach (var candidate in candidates.Where(x => x.Count > 1))
                        {
                            candidate.RemoveAll(rule => rule.FieldName == knownFieldName);
                        }
                    }
                }

                var knownFields = candidates.Select((x, fieldIndex) => new {fieldIndex, fieldName = x.Single().FieldName}).ToArray();

                Console.WriteLine(string.Join(NewLine, knownFields.Select(x => $"{x.fieldIndex}: {x.fieldName}")));

                var duplicated = knownFields.GroupBy(x => x.fieldName).Where(grp => grp.Count() > 1).Select(x => x.Key).ToArray();
                if (duplicated.Any())
                {
                    throw new InvalidOperationException($"Resolved all fields, but not all are distinct: {string.Join(", ", duplicated)}");
                }

                // Once you work out which field is which, look for the six fields on your ticket that start with the word departure.
                // What do you get if you multiply those six values together?
                return knownFields
                    .Where(x => x.fieldName.StartsWith("departure", StringComparison.InvariantCultureIgnoreCase))
                    .Select(x => YourTicket.Values[x.fieldIndex])
                    .Aggregate(1L, (acc, cur) => acc * cur);
            }

            private static IEnumerable<int> GetValuesForFieldIndex(int fieldIndex, IEnumerable<Ticket> validTickets) =>
                validTickets.Select(ticket => ticket.Values[fieldIndex]);

            private IEnumerable<FieldRule> GetCandidateRules(int fieldIndex, IEnumerable<Ticket> validTickets) =>
                Rules.Where(rule => GetValuesForFieldIndex(fieldIndex, validTickets).All(rule.IsValidValue));
        }

        public record FieldRule(
            string FieldName,
            IReadOnlyList<Range> Ranges)
        {
            private static readonly Regex ParseRuleRegex = new(@"(?<FieldName>[^:]+): ((?<Range>\d+-\d+)|( or ))+", RegexOptions.Compiled);

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

            public bool IsValid(IEnumerable<FieldRule> rules) => !GetInvalidValues(rules).Any();
        }
    }
}
