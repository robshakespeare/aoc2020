using System.Collections.Generic;
using System.Linq;

namespace AoC.Day16
{
    public record Ticket(IReadOnlyList<int> Values)
    {
        public static Ticket ParseTicket(string ticket) => new(ticket.Split(",").Select(int.Parse).ToArray());

        /// <summary>
        /// Each value is invalid if it does not match a single rule.
        /// </summary>
        public IEnumerable<int> GetInvalidValues(IEnumerable<FieldRule> rules) => Values.Where(value => !rules.Any(rule => rule.IsValidValue(value)));

        /// <summary>
        /// Returns true if this ticket is valid given the specified rules.
        /// </summary>
        public bool IsValid(IEnumerable<FieldRule> rules) => !GetInvalidValues(rules).Any();
    }
}
