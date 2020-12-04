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

        public Passport(Dictionary<string, string> dataItems)
        {
            string GetValue(string key) => dataItems.TryGetValue(key, out var value) ? value : "";

            DataItems = dataItems;
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
            IsBirthYearValid &&
            IsIssueYearValid &&
            IsExpirationYearValid &&
            IsHeightValid &&
            IsHairColorValid &&
            IsEyeColorValid &&
            IsValidPassportIdValid;

        private static bool IsValidNumber(string s, int min, int max) => int.TryParse(s, out var i) && i >= min && i <= max;
        private static readonly Regex ValidHeight = new(@"^(?<number>\d+)(?<unit>cm|in)$", RegexOptions.Compiled);
        private static readonly Regex ValidHairColor = new(@"^#[0-9a-f]{6}$", RegexOptions.Compiled);
        private static readonly string[] ValidEyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        private static readonly Regex ValidPassportId = new(@"^[0-9]{9}$", RegexOptions.Compiled);

        public bool IsBirthYearValid => IsValidNumber(BirthYear, 1920, 2002);

        public bool IsIssueYearValid => IsValidNumber(IssueYear, 2010, 2020);

        public bool IsExpirationYearValid => IsValidNumber(ExpirationYear, 2020, 2030);

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

        public bool IsHairColorValid => ValidHairColor.IsMatch(HairColor);

        public bool IsEyeColorValid => ValidEyeColors.Contains(EyeColor);

        public bool IsValidPassportIdValid => ValidPassportId.IsMatch(PassportId);

        public override string ToString() => string.Join(" ", DataItems.Select(x => x.Key + ":" + x.Value));
    }
}
