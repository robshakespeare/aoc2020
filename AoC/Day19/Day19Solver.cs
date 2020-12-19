using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day19
{
    public class Day19Solver : SolverBase
    {
        public override string DayName => "Monster Messages";

        private static long? Solve(string input)
        {
            var sections = input.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}");

            var ruleDefinitions = sections[0];
            var receivedMessages = sections[1];

            var rules = BuildRules(ruleDefinitions);

            const int ruleZeroId = 0;
            var ruleZero = rules[ruleZeroId];

            return receivedMessages.ReadLines()
                .Count(receivedMessage => ruleZero.IsMatch(receivedMessage, rules, out var remaining) && remaining.Length == 0);
        }

        protected override long? SolvePart1Impl(string input) => Solve(input);

        protected override long? SolvePart2Impl(string input)
        {
            input = input
                .Replace("8: 42", "8: 42 | 42 8")
                .Replace("11: 42 31", "11: 42 31 | 42 11 31");

            return Solve(input);
        }

        public interface IRule
        {
            int RuleId { get; }

            bool IsMatch(string input, Dictionary<int, IRule> rules, out string remaining);
        }

        public class BaseRule : IRule
        {
            public int RuleId { get; }
            private readonly char _c;

            public BaseRule(int ruleId, char c)
            {
                RuleId = ruleId;
                _c = c;
            }

            public bool IsMatch(string input, Dictionary<int, IRule> _, out string remaining)
            {
                if (input.StartsWith(_c))
                {
                    remaining = input[1..];
                    return true;
                }

                remaining = input;
                return false;
            }
        }

        public class Rule : IRule
        {
            public int RuleId { get; }
            private readonly int[][] _ruleSets;

            public Rule(int ruleId, int[][] ruleSets)
            {
                RuleId = ruleId;
                _ruleSets = ruleSets;

                var isValid = _ruleSets.Any() && _ruleSets.All(ruleSet => ruleSet.Any());
                if (!isValid)
                {
                    throw new InvalidOperationException($"Rule Id {ruleId} is invalid - has one or more empty sets?");
                }
            }

            public bool IsMatch(string input, Dictionary<int, IRule> rules, out string remaining)
            {
                // Any of the rule sets must match
                foreach (var ruleSet in _ruleSets)
                {
                    var setRemaining = input;

                    // Every rule in the set must match
                    var setSuccess = ruleSet.Length > 0;
                    foreach (var ruleId in ruleSet)
                    {
                        var setInput = setRemaining;
                        var rule = rules[ruleId];

                        var isMatch = rule.IsMatch(setInput, rules, out setRemaining);

                        if (!isMatch)
                        {
                            setSuccess = false;
                            break;
                        }
                    }

                    if (setSuccess)
                    {
                        remaining = setRemaining;
                        return true;
                    }
                }

                remaining = input;
                return false;
            }
        }

        private static readonly Regex ParseRawLine = new(
            @"^(?<ruleId>\d+): ((""(?<chr>a|b)"")|(?<subRules>.+))$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static Dictionary<int, IRule> BuildRules(string ruleDefinitions) =>
            ruleDefinitions.ReadLines()
                .Select(line =>
                {
                    var match = ParseRawLine.Match(line);
                    if (!match.Success)
                    {
                        throw new InvalidOperationException("Invalid raw line: " + line);
                    }

                    var ruleId = int.Parse(match.Groups["ruleId"].Value);

                    if (match.Groups["chr"].Success)
                    {
                        return new BaseRule(ruleId, match.Groups["chr"].Value[0]);
                    }

                    var subRules = match.Groups["subRules"].Value
                        .Split(" | ")
                        .Select(subRule => subRule.Split(' '))
                        .Select(subRuleIds => subRuleIds.Select(int.Parse).ToArray())
                        .ToArray();

                    return (IRule) new Rule(ruleId, subRules);
                })
                .OrderBy(x => x.RuleId)
                .ToDictionary(x => x.RuleId);
    }
}
