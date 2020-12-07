using System.Collections.Generic;
using System.Linq;

namespace AoC.Day7
{
    public class BagRules
    {
        private readonly BagRule[] _bagRules;
        private readonly Dictionary<string, BagRule> _bagRulesDictionary;

        public BagRules(params BagRule[] bagRules)
        {
            _bagRules = bagRules;
            _bagRulesDictionary = bagRules.ToDictionary(x => x.BagColor, x => x);
        }

        public static BagRules Parse(string input) => new(input.ReadLines().Select(BagRule.Parse).ToArray());

        /// <summary>
        /// Counts the number of bag colors that can eventually contain at least one specified color of bag.
        /// </summary>
        public long CountBagColorsCanContain(string bagColor) => _bagRules.Count(bagRule => CanContain(bagRule, bagColor));

        private bool CanContain(BagRule bagRule, string bagColor) =>
            bagRule.CanDirectlyContain(bagColor) ||
            bagRule.CanContainColorQuantity.Any(x => CanContain(_bagRulesDictionary[x.containingBagColor], bagColor));

        /// <summary>
        /// Counts the individual number of bags required inside the specified color of bag.
        /// </summary>
        public long CountBagsRequiredInside(string bagColor) =>
            _bagRulesDictionary[bagColor].CanContainColorQuantity.Sum(x => x.quantity + x.quantity * CountBagsRequiredInside(x.containingBagColor));
    }
}
