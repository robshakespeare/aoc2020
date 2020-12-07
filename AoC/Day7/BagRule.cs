using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day7
{
    public class BagRule
    {
        public string BagColor { get; }

        public IEnumerable<(string containingBagColor, int quantity)> CanContainColorQuantity { get; }

        private readonly Dictionary<string, int> _canContainColorQuantityDictionary;

        public BagRule(string bagColor, params (string containingBagColor, int quantity)[] canContainColorQuantity)
        {
            BagColor = bagColor;
            CanContainColorQuantity = canContainColorQuantity;
            _canContainColorQuantityDictionary = canContainColorQuantity.ToDictionary(x => x.containingBagColor, x => x.quantity);
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
