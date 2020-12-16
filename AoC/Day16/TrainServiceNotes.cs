using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day16
{
    public record TrainServiceNotes(IReadOnlyList<FieldRule> Rules, Ticket YourTicket, IReadOnlyList<Ticket> OtherTickets)
    {
        public static TrainServiceNotes Parse(string input)
        {
            var parts = input.NormalizeLineEndings().Split(Environment.NewLine + Environment.NewLine);
            if (!parts[1].StartsWith("your ticket:") || !parts[2].StartsWith("nearby tickets:"))
            {
                throw new InvalidOperationException("Ticket not valid: " + input);
            }

            return new TrainServiceNotes(
                FieldRule.ParseRules(parts[0]),
                Ticket.ParseTicket(parts[1].ReadLines().Skip(1).Single()),
                parts[2].ReadLines().Skip(1).Select(Ticket.ParseTicket).ToArray());
        }

        private IEnumerable<Ticket> AllTickets => new[] {YourTicket}.Concat(OtherTickets);

        /// <summary>
        /// Returns every invalid value in every invalid ticket.
        /// </summary>
        public IEnumerable<int> GetInvalidTicketValues() => AllTickets.SelectMany(ticket => ticket.GetInvalidValues(Rules));

        /// <summary>
        /// Returns all the valid tickets, including yours.
        /// </summary>
        public IEnumerable<Ticket> GetValidTickets() => AllTickets.Where(ticket => ticket.IsValid(Rules));

        /// <summary>
        /// Determines the correct order of the fields, and then looks for the six fields on your ticket that start with the word departure,
        /// and then multiplies those six values together and returns that value.
        /// </summary>
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

            var knownFields = candidates.Select((x, fieldIndex) => new { fieldIndex, fieldName = x.Single().FieldName }).ToArray();

            Console.WriteLine(string.Join(Environment.NewLine, knownFields.Select(x => $"{x.fieldIndex}: {x.fieldName}")));

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
}
