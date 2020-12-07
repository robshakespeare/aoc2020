using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day7
{
    public class Day7Solver : SolverBase
    {
        protected override long? SolvePart1Impl(string input) => BagRules.Parse(input).CountBagColorsCanContain("shiny gold");

        protected override long? SolvePart2Impl(string input)
        {
            return base.SolvePart2Impl(input);
        }
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
        public long CountBagColorsCanContain(string bagColor)
        {
            var validContainingBagColors = new HashSet<string>();

            foreach (var bagRule in _bagRules)
            {
                if (CanContain(bagRule, bagColor))
                {
                    validContainingBagColors.Add(bagRule.BagColor);
                }
            }

            return validContainingBagColors.Count;
        }

        private bool CanContain(BagRule bagRule, string bagColor)
        {
            if (bagRule.CanDirectlyContain(bagColor))
            {
                return true;
            }

            // or, if any of the child bags can contain the `bagColor`
            return bagRule.CanContainColorQuantity
                .Any(x => CanContain(_bagRulesDictionary[x.containingBagColor], bagColor));
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
