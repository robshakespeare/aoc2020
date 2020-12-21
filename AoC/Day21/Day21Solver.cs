using System;
using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace AoC.Day21
{
    public class Day21Solver : SolverBase
    {
        public override string DayName => "Allergen Assessment";

        /// <summary>
        /// Determine which ingredients cannot possibly contain any of the allergens in your list.
        /// How many times do any of those ingredients appear?
        /// </summary>
        protected override long? SolvePart1Impl(string input)
        {
            var foodItems = FoodItem.ParsePuzzleInput(input);

            var allAllergens = foodItems.SelectMany(x => x.Allergens).ToHashSet();
            Console.WriteLine("allAllergens: " + string.Join(", ", allAllergens));

            // For each known allergen, we can get all the sets of ingredients that they're known to be in
            // The INTERSECTION of all of those sets MUST contain the allergen
            var candidates = GetAllergensAndCandidateIngredients(allAllergens, foodItems).ToArray();
            while (candidates.Any(candidate => candidate.ingredientSet.Count > 1))
            {
                // Eliminate any whose count is 1
                foreach (var definite in candidates.Where(x => x.ingredientSet.Count == 1))
                {
                    var knownIngredient = definite.ingredientSet.Single();

                    foreach (var candidate in candidates.Where(x => x.ingredientSet.Count > 1))
                    {
                        candidate.ingredientSet.Remove(knownIngredient);
                    }
                }
            }

            var definiteIngredientToAllergenList = candidates
                .Select(x => new {x.allergen, ingredient = x.ingredientSet.Single()})
                .ToDictionary(x => x.ingredient, x => x.allergen);
            Console.WriteLine(
                $"{NewLine}definiteIngredientToAllergenList:{NewLine}{string.Join(NewLine, definiteIngredientToAllergenList.Select(x => $"{x.Key}: {x.Value}"))}");

            // Remove the definite allergenic ingredients from all our food items, and count the remaining ingredients
            var definiteAllergenicIngredients = definiteIngredientToAllergenList.Keys;

            return foodItems.Select(x => x.Ingredients)
                .SelectMany(ingredients => ingredients.Except(definiteAllergenicIngredients))
                .Count();
        }

        private static IEnumerable<(string allergen, HashSet<string> ingredientSet)> GetAllergensAndCandidateIngredients(
            IEnumerable<string> allAllergens,
            IReadOnlyList<FoodItem> foodItems)
        {
            Console.WriteLine($"{NewLine}candidates:");

            foreach (var allergen in allAllergens)
            {
                var ingredientSets = foodItems
                    .Where(foodItem => foodItem.Allergens.Contains(allergen))
                    .Select(foodItem => foodItem.Ingredients)
                    .ToArray();

                var ingredientSet = new HashSet<string>(ingredientSets.First());

                foreach (var otherIngredientSet in ingredientSets)
                {
                    ingredientSet.IntersectWith(otherIngredientSet);
                }

                Console.WriteLine($"{allergen}: " + string.Join(", ", ingredientSet));

                yield return (allergen, ingredientSet);
            }
        }

        /// <summary>
        /// Arrange the ingredients alphabetically by their allergen and
        /// separate them by commas to produce your canonical dangerous ingredient list.
        /// </summary>
        protected override long? SolvePart2Impl(string input)
        {
            var foodItems = FoodItem.ParsePuzzleInput(input);

            var allAllergens = foodItems.SelectMany(x => x.Allergens).ToHashSet();
            Console.WriteLine("allAllergens: " + string.Join(", ", allAllergens));

            // For each known allergen, we can get all the sets of ingredients that they're known to be in
            // The INTERSECTION of all of those sets MUST contain the allergen
            var candidates = GetAllergensAndCandidateIngredients(allAllergens, foodItems).ToArray();
            while (candidates.Any(candidate => candidate.ingredientSet.Count > 1))
            {
                // Eliminate any whose count is 1
                foreach (var definite in candidates.Where(x => x.ingredientSet.Count == 1))
                {
                    var knownIngredient = definite.ingredientSet.Single();

                    foreach (var candidate in candidates.Where(x => x.ingredientSet.Count > 1))
                    {
                        candidate.ingredientSet.Remove(knownIngredient);
                    }
                }
            }

            var definiteIngredientToAllergenList = candidates
                .Select(x => new { x.allergen, ingredient = x.ingredientSet.Single() })
                .ToDictionary(x => x.ingredient, x => x.allergen);
            Console.WriteLine(
                $"{NewLine}definiteIngredientToAllergenList:{NewLine}{string.Join(NewLine, definiteIngredientToAllergenList.Select(x => $"{x.Key}: {x.Value}"))}");

            // Remove the definite allergenic ingredients from all our food items, and count the remaining ingredients
            var definiteAllergenicIngredients = definiteIngredientToAllergenList.Keys;

            var part2Answer = string.Join(",", definiteIngredientToAllergenList.OrderBy(x => x.Value).Select(x => x.Key));
            Console.WriteLine(part2Answer);

            return null;

        }
    }
}
