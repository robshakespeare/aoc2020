using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sprache;

namespace AoC.Day19
{
    public class Day19Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input)
        {
            var sections = input.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}");

            var rules = sections[0];
            var receivedMessages = sections[1];
            var intermediaryLines = ResolveIntermediaryLines(rules);

            const int ruleId = 0;
            var rule0 = ResolveRule(ruleId, intermediaryLines).End();

            return receivedMessages.ReadLines().Count(receivedMessage => rule0.TryParse(receivedMessage).WasSuccessful);
        }

        protected override long? SolvePart2Impl(string input)
        {
            var sections = input.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}");

            var rules = sections[0];
            var receivedMessages = sections[1];
            var intermediaryLines = ResolveIntermediaryLines(rules);

            var part2Solver = new Part2Solver(intermediaryLines);

            return receivedMessages.ReadLines().Count(receivedMessage => part2Solver.Rule0.TryParse(receivedMessage).WasSuccessful);
        }

        public class Part2Solver
        {
            private Parser<string> Rule42 { get; }
            private Parser<string> Rule31 { get; }

            public Part2Solver(Dictionary<int, IntermediaryLine> intermediaryLines)
            {
                Rule42 = ResolveRule(42, intermediaryLines);
                Rule31 = ResolveRule(31, intermediaryLines);

                /*
                 * Top level rules:
                 *
                 * 0:  8 11
                 * 8:  42 | 42 8
                 * 11: 42 31 | 42 11 31
                 */
            }

            public Parser<string> Rule0 =>
                Rule8.Then(_ => Rule11).End();

            private Parser<string> Rule8 =>
                Rule42.Or(
                    Rule42.Then(_ => Rule8));

            private Parser<string> Rule11 =>
                Rule42.Then(_ => Rule31).Or(
                    Rule42.Then(_ => Rule11).Then(_ => Rule31));
        }

        private static readonly Regex ParseRawLine = new(
            @"^(?<ruleId>\d+): ((""(?<chr>a|b)"")|(?<subRules>.+))$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public record IntermediaryLine(int RuleId, Parser<string>? BaseRuleParser, int[][]? SubRules)
        {
        }

        public static Dictionary<int, IntermediaryLine> ResolveIntermediaryLines(string input) =>
            input.ReadLines()
                .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
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
                        return new IntermediaryLine(ruleId, Parse.Char(match.Groups["chr"].Value[0]).Once().Text(), null);
                    }

                    var subRules = match.Groups["subRules"].Value
                        .Split(" | ")
                        .Select(subRule => subRule.Split(' '))
                        .Select(subRuleIds => subRuleIds.Select(int.Parse).ToArray())
                        .ToArray();

                    return new IntermediaryLine(ruleId, null, subRules);
                })
                .OrderBy(x => x.RuleId)
                .ToDictionary(x => x.RuleId);

        public static Parser<string> ResolveRule(int ruleIdToResolve, Dictionary<int, IntermediaryLine> intermediaryLines)
        {
            //var parserCache = new Dictionary<int, Parser<string>>();

            return GetRule(ruleIdToResolve);

            Parser<string> GetRule(int ruleId)
            {
                //if (parserCache.TryGetValue(ruleId, out var existingRuleParser))
                //{
                //    return existingRuleParser;
                //}

                var (_, baseRuleParser, subRules) = intermediaryLines[ruleId];
                Parser<string>? parser = null;

                if (baseRuleParser != null)
                {
                    parser = baseRuleParser;
                }
                else if (subRules != null)
                {
                    parser = subRules
                        .Select(subRule => subRule.Aggregate<int, Parser<string>?>(
                            null,
                            (subParser, subRuleId) => subParser == null
                                ? subParser = GetRule(subRuleId)
                                : subParser.Then(_ => GetRule(subRuleId))))
                        .Aggregate(parser, (accParser, subParser) => accParser == null
                            ? subParser
                            : accParser.Or(subParser));

                    if (parser == null)
                    {
                        throw new InvalidOperationException("Invalid rule with ID {ruleId} - empty sub rules?");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Invalid rule with ID {ruleId} - it has no base rule or sub rules.");
                }

                //parserCache.Add(ruleId, parser);
                return parser;
            }
        }
    }
}
