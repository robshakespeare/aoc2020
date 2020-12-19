using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sprache;

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

            //var sections = input.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}");

            //var rules = sections[0];
            //var receivedMessages = sections[1];
            //var intermediaryLines = BuildRules(rules);

            //var part2Solver = new Part2Solver(intermediaryLines);

            //return receivedMessages.ReadLines().Count(receivedMessage => part2Solver.Rule0.TryParse(receivedMessage).WasSuccessful);
        }

        //public class Part2Solver
        //{
        //    private Parser<string> Rule42 { get; }
        //    private Parser<string> Rule31 { get; }

        //    public Part2Solver(Dictionary<int, IntermediaryLine> intermediaryLines)
        //    {
        //        Rule42 = ResolveRule(42, intermediaryLines);
        //        Rule31 = ResolveRule(31, intermediaryLines);

        //        /*
        //         * Top level rules:
        //         *
        //         * 0:  8 11
        //         * 8:  42 | 42 8
        //         * 11: 42 31 | 42 11 31
        //         */
        //    }

        //    public bool IsMatch(string receivedMessage)
        //    {
                
        //    }

        //    public Parser<string> Rule0 =>
        //        Rule42.AtLeastOnce().Then(_ => Rule11).End();

        //        //Rule8.Then(_ => Rule11).End();

        //    //private Parser<string> Rule8 =>
        //    //    Rule42.Or(
        //    //        Rule42.Then(_ => Rule8));

        //    private Parser<string> Rule11 =>
        //        Rule42.Then(_ => Rule31).Or(
        //            Rule42.Then(_ => Rule11).Then(_ => Rule31));
        //}

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

                //if (input.StartsWith(Match1))
                //{
                //    output = input[Match1.Length..];
                //    return true;
                //}

                //if (Match2 != null && input.StartsWith(Match2))
                //{
                //    output = input[Match2.Length..];
                //    return true;
                //}

                //output = input;
                //return false;
            }

            //public static Rule Create(string match1, string? match2)
            //{
            //    if (string.IsNullOrEmpty(match1))
            //    {
            //        throw new ArgumentException($"{nameof(match1)} is null or empty");
            //    }

            //    if (match2 != null && string.IsNullOrEmpty(match2))
            //    {
            //        throw new ArgumentException($"{nameof(match2)} is empty");
            //    }

            //    return new Rule(match1, match2);
            //}
        }

        private static readonly Regex ParseRawLine = new(
            @"^(?<ruleId>\d+): ((""(?<chr>a|b)"")|(?<subRules>.+))$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public record IntermediaryLine(int RuleId, Parser<string>? BaseRuleParser, int[][]? SubRules)
        {
        }

        public static Dictionary<int, IRule> BuildRules(string ruleDefinitions) =>
            ruleDefinitions.ReadLines()
                ////.TakeWhile(line => !string.IsNullOrWhiteSpace(line))
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

                    return (IRule)new Rule(ruleId, subRules);
                })
                .OrderBy(x => x.RuleId)
                .ToDictionary(x => x.RuleId);

        //public static Parser<string> ResolveRule(int ruleIdToResolve, Dictionary<int, IntermediaryLine> intermediaryLines)
        //{
        //    //var parserCache = new Dictionary<int, Parser<string>>();

        //    return GetRule(ruleIdToResolve);

        //    Parser<string> GetRule(int ruleId)
        //    {
        //        //if (parserCache.TryGetValue(ruleId, out var existingRuleParser))
        //        //{
        //        //    return existingRuleParser;
        //        //}

        //        var (_, baseRuleParser, subRules) = intermediaryLines[ruleId];
        //        Parser<string>? parser = null;

        //        if (baseRuleParser != null)
        //        {
        //            parser = baseRuleParser;
        //        }
        //        else if (subRules != null)
        //        {
        //            parser = subRules
        //                .Select(subRule => subRule.Aggregate<int, Parser<string>?>(
        //                    null,
        //                    (subParser, subRuleId) => subParser == null
        //                        ? subParser = GetRule(subRuleId)
        //                        : subParser.Then(_ => GetRule(subRuleId))))
        //                .Aggregate(parser, (accParser, subParser) => accParser == null
        //                    ? subParser
        //                    : accParser.Or(subParser));

        //            if (parser == null)
        //            {
        //                throw new InvalidOperationException("Invalid rule with ID {ruleId} - empty sub rules?");
        //            }
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException($"Invalid rule with ID {ruleId} - it has no base rule or sub rules.");
        //        }

        //        //parserCache.Add(ruleId, parser);
        //        return parser;
        //    }
        //}
    }
}
