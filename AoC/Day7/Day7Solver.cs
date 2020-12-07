using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day7
{
    public class Day7Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => BagRules.Parse(input).CountBagColorsCanContain("shiny gold");

        protected override long? SolvePart2Impl(string input) => BagRules.Parse(input).CountBagsRequiredInside("shiny gold");
    }

    public class BagRules
    {
        private readonly BagRule[] _bagRules;
        private readonly Dictionary<string, BagRule> _bagRulesDictionary;

        public BagRules(params BagRule[] bagRules)
        {
            _bagRules = bagRules;
            _bagRulesDictionary = bagRules.ToDictionary(
                x => x.BagColor,
                x => x);
        }

        /// <summary>
        /// Counts the number of bag colors that can eventually contain at least one specified color of bag.
        /// </summary>
        public long CountBagColorsCanContain(string bagColor) => _bagRules.Count(bagRule => CanContain(bagRule, bagColor));

        private bool CanContain(BagRule bagRule, string bagColor) =>
            bagRule.CanDirectlyContain(bagColor) ||
            bagRule.CanContainColorQuantity.Any(x => CanContain(_bagRulesDictionary[x.containingBagColor], bagColor));

        public long CountBagsRequiredInside(string bagColor)
        {
            var bagRule = _bagRulesDictionary[bagColor];
            long count = 0;

            foreach (var (containingBagColor, quantity) in bagRule.CanContainColorQuantity)
            {
                count += quantity;
                count += quantity * CountBagsRequiredInside(containingBagColor);
            }

            return count;
        }

        public static BagRules Parse(string input) => new(input.ReadLines().Select(BagRule.Parse).ToArray());
    }

    public class BagRule
    {
        public string BagColor { get; }

        public IEnumerable<(string containingBagColor, int quantity)> CanContainColorQuantity { get; }

        private readonly Dictionary<string, int> _canContainColorQuantityDictionary;

        public BagRule(string bagColor, params (string containingBagColor, int quantity)[] canContainColorQuantity)
        {
            BagColor = bagColor;
            CanContainColorQuantity = canContainColorQuantity;
            _canContainColorQuantityDictionary = canContainColorQuantity.ToDictionary(
                x => x.containingBagColor,
                x => x.quantity);
        }

        public bool CanDirectlyContain(string bagColor) => _canContainColorQuantityDictionary.ContainsKey(bagColor);

        private static readonly Regex ParseBag = new("^(?<bagColor>.+) bags contain (?<contents>.+)", RegexOptions.Compiled);

        private static readonly Regex ParseBagContents = new(@"((?<quantity>\d+) (?<containingBagColor>.+?) bag)+", RegexOptions.Compiled);

        public static BagRule Parse(string line)
        {
            var match = ParseBag.Match(line);

            if (!match.Success)
            {
                throw new InvalidOperationException("Invalid bag line: " + line);
            }

            var bagColor = match.Groups["bagColor"].Value;
            var contents = match.Groups["contents"].Value;

            return new BagRule(
                bagColor,
                ParseBagContents.Matches(contents)
                    .Select(content => (content.Groups["containingBagColor"].Value, int.Parse(content.Groups["quantity"].Value)))
                    .ToArray());
        }
    }
}
