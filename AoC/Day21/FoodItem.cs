using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day21
{
    public record FoodItem(IReadOnlySet<string> Ingredients, IReadOnlySet<string> Allergens)
    {
        public override string ToString() => $"{string.Join(" ", Ingredients)} (contains {string.Join(", ", Allergens)})";

        private static readonly Regex FoodItemLineRegex =
            new(@"^(?<ingredients>[^ ]+ )+\(contains (?<allergens>.+)\)$",
                RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static IReadOnlyList<FoodItem> ParsePuzzleInput(string input) =>
            input.ReadLines()
                .Select(line =>
                {
                    var match = FoodItemLineRegex.Match(line);
                    if (!match.Success)
                    {
                        throw new InvalidOperationException("Invalid Food Item: " + line);
                    }

                    var ingredients = match.Groups["ingredients"].Captures.Select(x => x.Value.Trim()).ToArray();
                    var ingredientsSet = ingredients.ToHashSet();
                    if (ingredientsSet.Count != ingredients.Length)
                    {
                        throw new InvalidOperationException(
                            $"Invalid Food Item, ingredients set count of {ingredientsSet.Count} does not match ingredients  count of {ingredients.Length}, for line: {line}");
                    }

                    var allergens = match.Groups["allergens"].Value.Split(", ").ToHashSet();
                    return new FoodItem(ingredientsSet, allergens);
                })
                .ToArray();
    }
}
