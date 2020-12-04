using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day4
{
    public static class PassportBatchParser
    {
        private static readonly Regex DataItemsRegex = new(@"(?<key>\w+):(?<value>#?\w+)", RegexOptions.Compiled);

        public static IEnumerable<Passport> ParseBatch(string batch) =>
            batch.NormalizeLineEndings()
                .Split($"{Environment.NewLine}{Environment.NewLine}")
                .Select(passportText => new Passport(
                    DataItemsRegex.Matches(passportText)
                        .Select(match => new
                        {
                            key = match.Groups["key"].Value,
                            value = match.Groups["value"].Value
                        })
                        .ToDictionary(kvp => kvp.key, kvp => kvp.value)));
    }
}
