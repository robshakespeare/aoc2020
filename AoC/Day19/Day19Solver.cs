using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Day19
{
    public class Day19Solver : SolverBase
    {
        public override string DayName => "";

        protected override long? SolvePart1Impl(string input)
        {
            var sections = input.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}");

            var ruleDefinitions = sections[0];
            var receivedMessages = sections[1];

            var intermediaryLines = ResolveToIntermediaryLines(ruleDefinitions);
            var ruleZeroRegex = new Regex(ResolveToRegex(ruleIdToResolve: 0, intermediaryLines), RegexOptions.Compiled);

            return receivedMessages.ReadLines().Count(receivedMessage => ruleZeroRegex.IsMatch(receivedMessage));
        }

        protected override long? SolvePart2Impl(string input)
        {
            input = input
                .Replace("8: 42", "8: 42 | 42 8")
                .Replace("11: 42 31", "11: 42 31 | 42 11 31");

            throw new NotImplementedException("rs-todo!");

            //var sections = input.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}");

            //var rulesParser = ResolveFirstRuleToParser(sections[0]);

            //var receivedMessages = sections[1];

            //return receivedMessages.ReadLines().Count(receivedMessage => rulesParser.TryParse(receivedMessage).WasSuccessful);
        }

        private static readonly Regex ParseRawLine = new(
            @"^(?<ruleId>\d+): ((""(?<chr>a|b)"")|(?<subRules>.+))$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public record IntermediaryLine(int RuleId, string? BaseRuleRegex, int[][]? SubRules)
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
                        return new IntermediaryLine(ruleId, match.Groups["chr"].Value, null);
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

        public string ResolveToRegex(int ruleIdToResolve, Dictionary<int, IntermediaryLine> intermediaryLines)
        {
            var regexs = new Dictionary<int, string>();

            return GetRegex(0);

            string GetRegex(int ruleId)
            {
                if (regexs.TryGetValue(ruleId, out var existingRegex))
                {
                    return existingRegex;
                }

                var (_, baseRuleRegex, subRules) = intermediaryLines[ruleId];
                string? regex = null;

                if (baseRuleRegex != null)
                {
                    regex = baseRuleRegex;
                }
                else if (subRules != null)
                {
                    // "Any" sub rule (i.e. OR)
                    var builder = new StringBuilder();
                    if (ruleId == 0)
                    {
                        builder.Append("^");
                    }
                    builder.Append("(");
                    var addSeparator = false;
                    foreach (var subRule in subRules)
                    {
                        // "All" ruleIds for the sub rule (i.e. AND)
                        var subRegex = string.Join("", subRule.Select(GetRegex));

                        if (addSeparator)
                        {
                            builder.Append("|");
                        }

                        builder.Append(subRegex);

                        addSeparator = true;
                    }

                    builder.Append(")");
                    if (ruleId == 0)
                    {
                        builder.Append("$");
                    }

                    regex = builder.ToString();
                }
                else
                {
                    throw new InvalidOperationException($"Invalid rule with ID {ruleId} - it has no base rule or sub rules.");
                }

                regexs.Add(ruleId, regex);
                return regex;
            }
        }
    }
}
