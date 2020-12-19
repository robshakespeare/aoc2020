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

            var rulesParser = ResolveFirstRuleToParser(sections[0]);

            var receivedMessages = sections[1];

            return receivedMessages.ReadLines().Count(receivedMessage => rulesParser.TryParse(receivedMessage).WasSuccessful);
        }

        protected override long? SolvePart2Impl(string input)
        {
            input = input
                .Replace("8: 42", "8: 42 | 42 8")
                .Replace("11: 42 31", "11: 42 31 | 42 11 31");

            var sections = input.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}");

            var rulesParser = ResolveFirstRuleToParser(sections[0]);

            var receivedMessages = sections[1];

            return receivedMessages.ReadLines().Count(receivedMessage => rulesParser.TryParse(receivedMessage).WasSuccessful);
        }

        private static readonly Regex ParseRawLine = new(
            @"^(?<ruleId>\d+): ((""(?<chr>a|b)"")|(?<subRules>.+))$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public record IntermediaryLine(int RuleId, Parser<string>? BaseRuleParser, int[][]? SubRules)
        {
        }

        public static Dictionary<int, IntermediaryLine> ResolveToIntermediaryLines(string input) =>
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

        public Parser<string> ResolveFirstRuleToParser(string input)
        {
            var intermediaryLines = ResolveToIntermediaryLines(input);
            var parserCache = new Dictionary<int, Parser<string>>();

            Parser<string> GetRuleParser(int ruleId)
            {
                if (parserCache.TryGetValue(ruleId, out var existingRuleParser))
                {
                    return existingRuleParser;
                }

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
                                ? subParser = GetRuleParser(subRuleId)
                                : subParser.Then(_ => GetRuleParser(subRuleId))))
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

                parserCache.Add(ruleId, parser);
                return parser;
            }

            return GetRuleParser(0).End();
        }
    }
}
