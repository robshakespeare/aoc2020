using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Day4
{
    public class Passport
    {
        public Passport(params (string key, string value)[] dataItems)
            : this((IEnumerable<(string key, string value)>) dataItems)
        {
        }

        public Passport(IEnumerable<(string key, string value)> dataItems)
        {
            DataItems = dataItems.ToDictionary(kvp => kvp.key, kvp => kvp.value);
        }

        public Dictionary<string, string> DataItems { get; }

        private const string BirthYearKey = "byr";
        private const string IssueYearKey = "iyr";
        private const string ExpirationYearKey = "eyr";
        private const string HeightKey = "hgt";
        private const string HairColorKey = "hcl";
        private const string EyeColorKey = "ecl";
        private const string PassportIdKey = "pid";

        /// <remarks>
        /// Field "cid" (Country ID) is optional.
        /// </remarks>
        public bool HasRequiredFields =>
            DataItems.ContainsKey(BirthYearKey) && // (Birth Year)
            DataItems.ContainsKey(IssueYearKey) && // (Issue Year)
            DataItems.ContainsKey(ExpirationYearKey) && // (Expiration Year)
            DataItems.ContainsKey(HeightKey) && // (Height)
            DataItems.ContainsKey(HairColorKey) && // (Hair Color)
            DataItems.ContainsKey(EyeColorKey) && // (Eye Color)
            DataItems.ContainsKey(PassportIdKey); // (Passport ID)

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

        private bool IsValidYear(string key, int min, int max) => DataItems.TryGetValue(key, out var s) && IsValidNumber(s, min, max);

        public bool IsBirthYearValid => IsValidYear(BirthYearKey, 1920, 2002);

        public bool IsIssueYearValid => IsValidYear(IssueYearKey, 2010, 2020);

        public bool IsExpirationYearValid => IsValidYear(ExpirationYearKey, 2020, 2030);

        private static readonly Regex ValidHeight = new(@"^\d+(cm|in)$", RegexOptions.Compiled);

        public bool IsHeightValid => DataItems.TryGetValue(HeightKey, out var hgt) &&
                                     ValidHeight.IsMatch(hgt) &&
                                     hgt switch
                                     {
                                         _ when hgt.EndsWith("cm") => IsValidNumber(new string(hgt.TakeWhile(char.IsNumber).ToArray()), 150, 193),
                                         _ when hgt.EndsWith("in") => IsValidNumber(new string(hgt.TakeWhile(char.IsNumber).ToArray()), 59, 76),
                                         _ => false
                                     };

        private static readonly Regex ValidHairColor = new(@"^#[0-9a-f]{6}$", RegexOptions.Compiled);

        public bool IsHairColorValid => DataItems.TryGetValue(HairColorKey, out var hcl) &&
                                        ValidHairColor.IsMatch(hcl);

        public bool IsEyeColorValid => DataItems.TryGetValue(EyeColorKey, out var ecl) &&
                                       ecl switch
                                       {
                                           "amb" => true,
                                           "blu" => true,
                                           "brn" => true,
                                           "gry" => true,
                                           "grn" => true,
                                           "hzl" => true,
                                           "oth" => true,
                                           _ => false
                                       };

        private static readonly Regex ValidPassportId = new(@"^[0-9]{9}$", RegexOptions.Compiled);

        public bool IsValidPassportIdValid => DataItems.TryGetValue(PassportIdKey, out var pid) &&
                                              ValidPassportId.IsMatch(pid);

        public override string ToString() => string.Join(" ", DataItems.Select(x => x.Key + ":" + x.Value));
    }
}
