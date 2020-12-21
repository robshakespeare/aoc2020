using System;
using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace AoC.Day21
{
    public class Day21Solver : SolverBase<int, string>
    {
        public override string DayName => "Allergen Assessment";

        /// <summary>
        /// Determine which ingredients cannot possibly contain any of the allergens in your list.
        /// How many times do any of those ingredients appear?
        /// </summary>
        protected override int SolvePart1Impl(string input)
        {
            var foodItems = FoodItem.ParsePuzzleInput(input);

            var allergenAndDefiniteIngredientList = GetAllergenAndDefiniteIngredientList(foodItems);

            // Remove the definite allergenic ingredients from all our food items, and count the remaining ingredients
            var definiteAllergenicIngredients = allergenAndDefiniteIngredientList.Select(x => x.ingredient);

            return foodItems.Select(x => x.Ingredients)
                .SelectMany(ingredients => ingredients.Except(definiteAllergenicIngredients))
                .Count();
        }

        /// <summary>
        /// Arrange the ingredients alphabetically by their allergen and
        /// separate them by commas to produce your canonical dangerous ingredient list.
        /// </summary>
        protected override string SolvePart2Impl(string input)
        {
            var foodItems = FoodItem.ParsePuzzleInput(input);

            var allergenAndDefiniteIngredientList = GetAllergenAndDefiniteIngredientList(foodItems);

            return string.Join(",", allergenAndDefiniteIngredientList.OrderBy(x => x.allergen).Select(x => x.ingredient));
        }

        private static IReadOnlyList<(string allergen, string ingredient)> GetAllergenAndDefiniteIngredientList(IReadOnlyList<FoodItem> foodItems)
        {
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

            var allergenAndDefiniteIngredientList = candidates
                .Select(x => (x.allergen, ingredient: x.ingredientSet.Single()))
                .ToArray();

            Console.WriteLine(
                $"{NewLine}allergenAndDefiniteIngredientList:{NewLine}{string.Join(NewLine, allergenAndDefiniteIngredientList.Select(x => $"{x.allergen}: {x.ingredient}"))}");

            return allergenAndDefiniteIngredientList;
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
    }
}
