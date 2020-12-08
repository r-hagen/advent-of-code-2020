using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class HandyHaversacks
    {
        public int Solve1(string[] input, string search)
        {
            var bags = ParseInput(input);
            return bags.Count(bag => bag.CanHold(search));
        }

        public int Solve2(string[] input, string search)
        {
            var bags = ParseInput(input);
            return bags.Single(bag => bag.HasColor(search)).MustContain();
        }

        public List<Bag> ParseInput(string[] input)
        {
            var bags = new List<Bag>();

            Func<string, Bag> getOrAdd = (string color) =>
            {
                if (bags.Any(b => b.HasColor(color)))
                    return bags.Single(b => b.HasColor(color));

                var bag = new Bag(color);
                bags.Add(bag);

                return bag;
            };

            foreach (var line in input)
            {
                var lineTokens = line
                    .Replace("bags", "")
                    .Replace("bag", "")
                    .Replace("contain", "")
                    .Replace(",", "")
                    .Replace(".", "")
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var bag = getOrAdd(string.Join(' ', lineTokens.Take(2)));

                if (line.Contains("no other"))
                    continue;

                var remainingTokens = lineTokens.Skip(2);

                while (remainingTokens.Count() > 0)
                {
                    var specTokens = remainingTokens.Take(3).ToArray();

                    var amount = int.Parse(specTokens.First());
                    var color = string.Join(' ', specTokens.Skip(1).Take(2));

                    var specBag = getOrAdd(color);
                    bag.CanContain(specBag, amount);

                    remainingTokens = remainingTokens.Skip(3);
                }
            }

            return bags;
        }

        public class Bag
        {
            private string _color;
            private List<Tuple<Bag, int>> _bags = new List<Tuple<Bag, int>>();

            public Bag(string color)
            {
                _color = color ?? throw new ArgumentNullException(nameof(color));
            }

            public bool HasColor(string color)
            {
                return _color.Equals(color);
            }

            public void CanContain(Bag bag, int amount)
            {
                _bags.Add(new(bag, amount));
            }

            public bool CanHold(string color)
            {
                return _bags.Any(t => t.Item1.HasColor(color)) || _bags.Any(t => t.Item1.CanHold(color));
            }

            public int MustContain()
            {
                if (_bags.Count == 0)
                    return 0;

                var otherBags = _bags.Sum(t => t.Item2);
                var otherBagsForOtherBags = _bags.Sum(t => t.Item1.MustContain() * t.Item2);

                return otherBags + otherBagsForOtherBags;
            }
        }
    }
}
