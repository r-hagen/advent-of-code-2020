using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    public class MonsterMessages
    {
        private readonly Dictionary<int, string> _rules = new();
        private const string RuleFormat = "({0})";

        public int Solve1(params string[] input)
        {
            var separator = input
                .Select((line, index) => (line, index))
                .First(t => t.line.Equals(string.Empty))
                .index;

            var ruleLines = input.Take(separator).ToArray();
            var messageLines = input.Skip(separator + 1).ToArray();

            var pattern = BuildRegexPattern1(ruleLines);
            var regex = new Regex(pattern);

            return messageLines.Count(msg => regex.Match(msg).Success);
        }

        public string BuildRegexPattern1(IEnumerable<string> ruleLines)
        {
            var ruleRegex = new Regex(@"^(?<rule>\d+): (?<spec>.*)$");

            foreach (var line in ruleLines)
            {
                var match = ruleRegex.Match(line);
                if (!match.Success) throw new NotSupportedException(line);

                var rule = int.Parse(match.Groups["rule"].Value);
                var spec = match.Groups["spec"].Value;

                _rules.Add(rule, spec);
            }

            return $"^{buildRulePattern1(0)}$";
        }

        private string buildRulePattern1(int ruleId)
        {
            var rule = _rules[ruleId];

            if (isLetter(rule, out var letter))
                return letter.ToString();

            var subRules = rule.Split("|", StringSplitOptions.RemoveEmptyEntries);
            var subRulesRegex = new List<string>();

            foreach (var subRule in subRules)
            {
                var subRuleIds = subRule
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse);

                var sb = new StringBuilder();

                foreach (var subId in subRuleIds)
                    sb.Append(buildRulePattern1(subId));

                subRulesRegex.Add(string.Format(RuleFormat, sb));
            }

            var joined = string.Join("|", subRulesRegex);
            var result = string.Format(RuleFormat, joined);

            return result;
        }

        public int Solve2(params string[] input)
        {
            var separator = input
                .Select((line, index) => (line, index))
                .First(t => t.line.Equals(string.Empty))
                .index;

            var ruleLines = input.Take(separator).ToArray();
            var messageLines = input.Skip(separator + 1).ToArray();

            var pattern = BuildRegexPattern2(ruleLines);
            var regex = new Regex(pattern);

            return messageLines.Count(msg => regex.Match(msg).Success);
        }

        public string BuildRegexPattern2(IEnumerable<string> ruleLines)
        {
            var ruleRegex = new Regex(@"^(?<rule>\d+): (?<spec>.*)$");

            foreach (var line in ruleLines)
            {
                var match = ruleRegex.Match(line);
                if (!match.Success) throw new NotSupportedException(line);

                var rule = int.Parse(match.Groups["rule"].Value);
                var spec = match.Groups["spec"].Value;

                _rules.Add(rule, spec);
            }

            return $"^{buildRulePattern2(0)}$";
        }

        private string buildRulePattern2(int ruleId)
        {
            if (ruleId == 8)
            {
                var eight = buildRulePattern2(42) + "+";
                return string.Format(RuleFormat, eight);
            }

            if (ruleId == 11)
            {
                var parts = new List<string>();

                for (var i = 1; i < 10; i++)
                    parts.Add(buildRulePattern2(42) + "{" + i + "}" + buildRulePattern2(31) + "{" + i + "}");

                return string.Format(RuleFormat, string.Join("|", parts));
            }

            var rule = _rules[ruleId];

            if (isLetter(rule, out var letter))
                return letter.ToString();

            var subRules = rule.Split("|", StringSplitOptions.RemoveEmptyEntries);
            var subRulesRegex = new List<string>();

            foreach (var subRule in subRules)
            {
                var subRuleIds = subRule
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                var sb = new StringBuilder();

                foreach (var subId in subRuleIds)
                    sb.Append(buildRulePattern2(subId));

                subRulesRegex.Add(string.Format(RuleFormat, sb));
            }

            var joined = string.Join("|", subRulesRegex);
            var result = string.Format(RuleFormat, joined);

            return result;
        }

        private bool isLetter(string rule, out char letter)
        {
            var charRegex = new Regex("\"");
            letter = charRegex.Replace(rule, "")[0];
            return char.IsLetter(letter);
        }
    }
}
