using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles
{
    public class TicketTranslation
    {
        private Dictionary<string, NumberRange[]> _rules = new();
        private int[] _your;
        private int[][] _nearby;

        public int Solve(string[] input)
        {
            parseInput(input);

            var result = 0;

            foreach (var ticket in _nearby)
            {
                var invalidNumbers = findInvalids(ticket);
                if (invalidNumbers.Any())
                    result += invalidNumbers.Sum();
            }

            return result;
        }

        private int[] findInvalids(int[] ticket)
        {
            var invalid = new List<int>();

            foreach (var number in ticket)
            {
                if (!getRules().Any(r => r.IsValid(number)))
                    invalid.Add(number);
            }

            return invalid.ToArray();
        }

        private void parseInput(string[] input)
        {
            var splitIndices = input
                .Select((line, index) => (Line: line, Index: index))
                .Where(x => string.IsNullOrEmpty(x.Line))
                .Select(x => x.Index)
                .ToArray();

            if (splitIndices.Length != 2) throw new InvalidOperationException();

            // parse rules
            var ruleLines = input.Take(splitIndices[0]).ToArray();
            foreach (var r in ruleLines)
                parseRule(r);

            // parse your ticket
            var yourLines = input.Skip(splitIndices[0] + 1).Take(2).ToArray();
            _your = yourLines[1]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s))
                .ToArray();

            // parse nearby tickets
            var nearbyLines = input.Skip(splitIndices[1] + 1).ToArray();
            _nearby = new int[nearbyLines.Length - 1][];
            for (var i = 1; i < nearbyLines.Length; i++)
            {
                var tmp = nearbyLines[i]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s))
                    .ToArray();

                _nearby[i - 1] = tmp;
            }
        }

        private void parseRule(string ruleLine)
        {
            var regex = new Regex("^(?<name>.+): (?<range>.+)$");

            var match = regex.Match(ruleLine);
            if (!match.Success) throw new InvalidOperationException();

            var ruleRanges = match.Groups["range"].Value.Split("or", StringSplitOptions.RemoveEmptyEntries)
                .Select(r => new NumberRange(r))
                .ToArray();

            var ruleName = match.Groups["name"].Value;

            if (_rules.ContainsKey(ruleName)) throw new InvalidOperationException();

            _rules.Add(ruleName, ruleRanges);
        }

        private NumberRange[] getRules()
        {
            // flatten all rules into single array
            return _rules.Values
                .SelectMany(a => a.Select(b => b))
                .ToArray();
        }

        public bool isValid(int[] ticket)
        {
            return findInvalids(ticket).Length == 0;
        }

        public ulong Solve2(string[] input)
        {
            parseInput(input);

            var validTickets = _nearby
                .Where(ticket => isValid(ticket))
                .ToArray();

            var rulesWithColumnCandidates = new Dictionary<string, List<int>>();

            foreach (var (rule, ranges) in _rules)
            {
                var validColumns = new List<int>();

                for (var column = 0; column < validTickets[0].Length; column++)
                {
                    var columnNumbers = validTickets
                        .Select(ticket => ticket[column])
                        .ToArray();

                    if (columnNumbers.All(number => ranges.Any(range => range.IsValid(number))))
                        validColumns.Add(column);
                }

                rulesWithColumnCandidates.Add(rule, validColumns);
            }

            var ruleColumns = new Dictionary<string, int>();

            foreach (var (rule, columns) in rulesWithColumnCandidates
                .OrderBy(rule => rule.Value.Count))
            {
                var column = columns.Single();

                ruleColumns.Add(rule, column);

                foreach (var (_, others) in rulesWithColumnCandidates)
                    others.Remove(column);
            }

            return ruleColumns.Where(m =>
                    m.Key.StartsWith("departure", StringComparison.OrdinalIgnoreCase))
                .Select(m => (ulong)_your[m.Value])
                .Aggregate(1ul, (a, b) => a * b);
        }
    }

    public record NumberRange
    {
        public int Lower { get; private set; }
        public int Upper { get; private set; }

        public NumberRange(string range)
        {
            var numbers = range
                .Split('-', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s))
                .ToArray();

            Lower = numbers[0];
            Upper = numbers[1];
        }

        public bool IsValid(int number)
        {
            return (number >= Lower) && (number <= Upper);
        }

        public bool IsValid(int[] numbers)
        {
            return numbers.All(number => IsValid(number));
        }

        public override string ToString()
        {
            return $"{Lower}-{Upper}";
        }
    }
}
