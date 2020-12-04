using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day4
{
    public class Passport
    {
        public string BirthYear { get; }
        public string IssueYear { get; }
        public string ExpirationYear { get; }
        public string Height { get; }
        public string HairColor { get; }
        public string EyeColor { get; }
        public string PassportId { get; }
        public string CountryId { get; }

        public Passport(IEnumerable<(string key, string value)> dataItems)
        {
            DataItems = dataItems.ToDictionary(kvp => kvp.key, kvp => kvp.value);

            string GetValue(string key) => DataItems.TryGetValue(key, out var value) ? value : "";

            BirthYear = GetValue("byr");
            IssueYear = GetValue("iyr");
            ExpirationYear = GetValue("eyr");
            Height = GetValue("hgt");
            HairColor = GetValue("hcl");
            EyeColor = GetValue("ecl");
            PassportId = GetValue("pid");
            CountryId = GetValue("cid");
        }

        public Dictionary<string, string> DataItems { get; }

        /// <remarks>
        /// Field "cid" (Country ID) is optional.
        /// </remarks>
        public bool HasRequiredFields => new[]
        {
            BirthYear,
            IssueYear,
            ExpirationYear,
            Height,
            HairColor,
            EyeColor,
            PassportId
        }.All(value => !string.IsNullOrEmpty(value));

        public bool IsValid =>
            HasRequiredFields &&
            IsValidNumber(BirthYear, 1920, 2002) &&
            IsValidNumber(IssueYear, 2010, 2020) &&
            IsValidNumber(ExpirationYear, 2020, 2030) &&
            IsHeightValid &&
            ValidHairColor.IsMatch(HairColor) &&
            ValidEyeColors.Contains(EyeColor) &&
            ValidPassportId.IsMatch(PassportId);

        private static bool IsValidNumber(string s, int min, int max) => int.TryParse(s, out var i) && i >= min && i <= max;

        private static readonly Regex ValidHeight = new(@"^(?<number>\d+)(?<unit>cm|in)$", RegexOptions.Compiled);

        public bool IsHeightValid
        {
            get
            {
                var match = ValidHeight.Match(Height);
                return match.Success &&
                       match.Groups["unit"].Value switch
                       {
                           "cm" => IsValidNumber(match.Groups["number"].Value, 150, 193),
                           "in" => IsValidNumber(match.Groups["number"].Value, 59, 76),
                           _ => false
                       };
            }
        }

        private static readonly Regex ValidHairColor = new(@"^#[0-9a-f]{6}$", RegexOptions.Compiled);

        private static readonly string[] ValidEyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        private static readonly Regex ValidPassportId = new(@"^[0-9]{9}$", RegexOptions.Compiled);

        public override string ToString() => string.Join(" ", DataItems.Select(x => x.Key + ":" + x.Value));

        #region Parsing

        private static readonly Regex ParseDataItemsRegex = new(@"(?<key>\S+):(?<value>\S+)", RegexOptions.Compiled);

        public static Passport Parse(string passport) =>
            new(ParseDataItemsRegex.Matches(passport).Select(match => (match.Groups["key"].Value, match.Groups["value"].Value)));

        public static IEnumerable<Passport> ParseBatch(string batch) =>
            batch.NormalizeLineEndings().Split($"{Environment.NewLine}{Environment.NewLine}").Select(Parse);

        #endregion
    }
}
