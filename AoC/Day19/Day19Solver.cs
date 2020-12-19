using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MoreLinq;

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
                .Count(receivedMessage => );

            //return receivedMessages.ReadLines()
            //    .Count(receivedMessage => ruleZero.IsMatch(receivedMessage, rules, out var remaining) && remaining.Length == 0);
        }

        private static bool IsMatch(IRule rootRule, Dictionary<int, IRule> rules, string receivedMessage)
        {
            var matches = MoreEnumerable.TraverseBreadthFirst(
                (rule: rootRule, remainers: new[] { receivedMessage }),
                node => Matches(node, rules));

            return matches?.Any(m => m.remainers);
        }

        //private static bool IsMatch(IRule rootRule, Dictionary<int, IRule> rules, string receivedMessage)
        //{
        //    var matches = MoreEnumerable.TraverseBreadthFirst(
        //        (rule: rootRule, remainers: new[] { receivedMessage }),
        //        node => Matches(node, rules));

        //    return matches?.Any(m => m.remainers);
        //}

        private static IEnumerable<(IRule rule, string remaining)> Matches((IRule rule, string remaining) node, Dictionary<int, IRule> rules)
        {
            foreach (var ruleSet in node.rule.ChildRuleSets)
            {
                // Every rule in the set must match
                foreach (var ruleId in ruleSet)
                {
                    var rule = rules[ruleId];
                    if (rule.IsMatch(node.remaining, out var newRemaining))
                    {
                        yield return (rule, newRemaining);
                    }
                }
            }

            //node.rule
        }

        //private static IEnumerable<(IRule rule, string[] remainers)> Matches((IRule rule, string[] remainers) node, Dictionary<int, IRule> rules)
        //{
        //    foreach (var remaining in node.remainers)
        //    {
        //        foreach (var ruleSet in node.rule.ChildRuleSets)
        //        {
        //            foreach (var ruleId in ruleSet)
        //            {
        //                var rule = rules[ruleId];
        //                if (rule.IsMatch(remaining, out var newRemaining))
        //                {
        //                    yield return (rule, new[] { newRemaining });
        //                }
        //            }
        //        }
        //    }

        //    //node.rule
        //}

        protected override long? SolvePart1Impl(string input) => Solve(input);

        protected override long? SolvePart2Impl(string input)
        {
            input = new Regex("^8: 42$").Replace(input, "8: 42 | 42 8");
            input = new Regex("^11: 42 31$").Replace(input, "11: 42 31 | 42 11 31");

            ////input = input
            ////    .Replace("8: 42", "8: 42 | 42 8")
            ////    .Replace("11: 42 31", "11: 42 31 | 42 11 31");

            return Solve(input);
        }

        public interface IRule
        {
            int RuleId { get; }

            //bool IsMatch(string input, Dictionary<int, IRule> rules, out string remaining);

            bool IsMatch(string input, out string remaining);

            int[][] ChildRuleSets { get; }
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

            public override string ToString() => $"{RuleId}: \"{_c}\"";

            public bool IsMatch(string input, out string remaining)
            {
                if (input.StartsWith(_c))
                {
                    remaining = input[1..];
                    return true;
                }

                remaining = input;
                return false;
            }

            public int[][] ChildRuleSets => Array.Empty<int[]>();

            //public bool IsMatch(string input, Dictionary<int, IRule> _, out string remaining)
            //{
            //    if (input.StartsWith(_c))
            //    {
            //        remaining = input[1..];
            //        return true;
            //    }

            //    remaining = input;
            //    return false;
            //}
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

            public bool IsMatch(string input, out string remaining)
            {
                throw new NotImplementedException();
            }

            public int[][] ChildRuleSets { get; }

            public override string ToString() => $"{RuleId}: {string.Join(" | ", _ruleSets.Select(ruleSet => string.Join(" ", ruleSet)))}";

            //public bool IsMatch(string input, Dictionary<int, IRule> rules, out string remaining)
            //{
            //    var possibleBranches = new List<()>()

            //    // Any of the rule sets must match
            //    foreach (var ruleSet in _ruleSets)
            //    {
            //        var setRemaining = input;

            //        // Every rule in the set must match
            //        var setSuccess = ruleSet.Length > 0;
            //        foreach (var ruleId in ruleSet)
            //        {
            //            var setInput = setRemaining;
            //            var rule = rules[ruleId];

            //            var isMatch = rule.IsMatch(setInput, rules, out setRemaining);

            //            if (!isMatch)
            //            {
            //                Console.WriteLine($"No match: {new { setInput, setRemaining, rule }}");
            //                setSuccess = false;
            //                break;
            //            }
            //        }

            //        if (setSuccess)
            //        {
            //            remaining = setRemaining;
            //            Console.WriteLine($"Set success: {new { ruleSet = string.Join(" ", ruleSet), input, remaining }}");
            //            return true; // isntead of returning here, we want to make sure the entire branch would match
            //        }
            //    }

            //    remaining = input;
            //    return false;
            //}
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
