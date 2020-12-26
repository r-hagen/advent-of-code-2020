using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    public class AllergenAssessment
    {
        private static Dictionary<string, HashSet<string>> _allAllergens;
        private static HashSet<string> _okIngredients;

        public static string Solve1(string[] foodInput)
        {
            var foodRegex = new Regex(@"(?<ingredients>[\w\s]+)((\(contains (?<allergens>.*)\))?|$)");

            _allAllergens = new Dictionary<string, HashSet<string>>();
            var allIngredients = new HashSet<string>();
            var foodIngredients = new List<string[]>();

            foreach (var foodLine in foodInput)
            {
                var match = foodRegex.Match(foodLine);
                if (!match.Success) throw new NotSupportedException();

                var ingredients = match.Groups["ingredients"].Value
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                allIngredients.UnionWith(ingredients);
                foodIngredients.Add(ingredients.ToArray());

                var allergens = match.Groups["allergens"].Value
                    .Replace(",", "")
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                foreach (var allergen in allergens)
                    if (!_allAllergens.ContainsKey(allergen))
                        _allAllergens.Add(allergen, new HashSet<string>(ingredients));
                    else
                        _allAllergens[allergen].IntersectWith(ingredients);
            }

            _okIngredients = new HashSet<string>();

            foreach (var ingredient in allIngredients)
            {
                var ok = !_allAllergens.Values.Any(x => x.Contains(ingredient));
                if (ok) _okIngredients.Add(ingredient);
            }

            var count = 0;

            foreach (var food in foodIngredients)
            foreach (var foodIngredient in food)
                if (_okIngredients.Contains(foodIngredient))
                    count += 1;

            return count.ToString();
        }

        public static string Solve2(string[] foodInput)
        {
            Solve1(foodInput);

            while (_allAllergens.Values.Any(x => x.Count > 1))
            {
                var allergensOrdered = _allAllergens
                    .OrderBy(kvp => kvp.Value.Count);

                foreach (var allergen in allergensOrdered)
                {
                    if (allergen.Value.Count != 1) continue;

                    var others = _allAllergens.Where(kvp => kvp.Key != allergen.Key).ToList();
                    foreach (var other in others)
                        other.Value.Remove(allergen.Value.Single());
                }
            }

            var result = string.Join(",", _allAllergens
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value.Single()));

            return result;
        }
    }
}