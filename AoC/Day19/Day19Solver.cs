using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Day19
{
    public class Day19Solver : SolverBase
    {
        public override string DayName => "Monster Messages";

        private static (Dictionary<int, RuleDefinition> ruleDefinitions, string receivedMessages) ParsePuzzleInput(string input)
        {
            var sections = input.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}");
            return (ResolveToRuleDefinitions(sections[0]), sections[1]);
        }

        protected override long? SolvePart1Impl(string input)
        {
            var (ruleDefinitions, receivedMessages) = ParsePuzzleInput(input);

            var ruleZeroRegex = new Regex($"^{ResolveRegex(ruleIdToResolve: 0, ruleDefinitions)}$", RegexOptions.Compiled);

            return receivedMessages.ReadLines().Count(receivedMessage => ruleZeroRegex.IsMatch(receivedMessage));
        }

        protected override long? SolvePart2Impl(string input)
        {
            var (ruleDefinitions, receivedMessages) = ParsePuzzleInput(input);

            var ruleZeroRegex = BuildPart2RuleZeroRegex(ruleDefinitions);

            return receivedMessages.ReadLines().Count(receivedMessage => ruleZeroRegex.IsMatch(receivedMessage));
        }

        public static Regex BuildPart2RuleZeroRegex(Dictionary<int, RuleDefinition> ruleDefinitions)
        {
            var rule42 = ResolveRegex(42, ruleDefinitions);
            var rule31 = ResolveRegex(31, ruleDefinitions);

            /*
             *  .Replace("8: 42", "8: 42 | 42 8")
             *  .Replace("11: 42 31", "11: 42 31 | 42 11 31");
             *
             * The Part 2 replacements result in the following Top level rules:
             *     0:  8 11
             *     8:  42 | 42 8  (essentially one or more rule 42s)
             *     11: 42 31 | 42 11 31  (back inside itself, always with 42 at beginning, 31 at end)
             */

            var rule8 = $"({rule42})+";
            var rule11 = string.Join("|", Enumerable.Range(1, 10).Select(repeat => $"(({rule42}){{{repeat}}}({rule31}){{{repeat}}})"));

            return new Regex($"^({rule8})({rule11})$", RegexOptions.Compiled);
        }

        private static readonly Regex ParseRawLine = new(
            @"^(?<ruleId>\d+): ((""(?<chr>a|b)"")|(?<subRules>.+))$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public record RuleDefinition(int RuleId, string? BaseRuleRegex, int[][]? SubRules)
        {
        }

        public static Dictionary<int, RuleDefinition> ResolveToRuleDefinitions(string input) =>
            input.ReadLines()
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
                        return new RuleDefinition(ruleId, match.Groups["chr"].Value, null);
                    }

                    var subRules = match.Groups["subRules"].Value
                        .Split(" | ")
                        .Select(subRule => subRule.Split(' '))
                        .Select(subRuleIds => subRuleIds.Select(int.Parse).ToArray())
                        .ToArray();

                    return new RuleDefinition(ruleId, null, subRules);
                })
                .OrderBy(x => x.RuleId)
                .ToDictionary(x => x.RuleId);

        public static string ResolveRegex(int ruleIdToResolve, Dictionary<int, RuleDefinition> ruleDefinitions)
        {
            var regexs = new Dictionary<int, string>();

            return GetRegex(ruleIdToResolve);

            string GetRegex(int ruleId)
            {
                if (regexs.TryGetValue(ruleId, out var existingRegex))
                {
                    return existingRegex;
                }

                var (_, baseRuleRegex, subRules) = ruleDefinitions[ruleId];
                string? regex = null;

                if (baseRuleRegex != null)
                {
                    regex = baseRuleRegex;
                }
                else if (subRules != null)
                {
                    // "Any" sub rule (i.e. OR)
                    var builder = new StringBuilder();

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
