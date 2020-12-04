using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace AoC.Day4
{
    // rs-todo: clean
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

        /// <remarks>
        /// Field "cid" (Country ID) is optional.
        /// </remarks>
        public bool IsValid =>
            DataItems.ContainsKey("byr") && // (Birth Year)
            DataItems.ContainsKey("iyr") && // (Issue Year)
            DataItems.ContainsKey("eyr") && // (Expiration Year)
            DataItems.ContainsKey("hgt") && // (Height)
            DataItems.ContainsKey("hcl") && // (Hair Color)
            DataItems.ContainsKey("ecl") && // (Eye Color)
            DataItems.ContainsKey("pid"); // (Passport ID)
    }

    public static class PassportBatchParser
    {
        private static readonly Parser<string> DataItemDelimiter =
            Parse.Char(' ').Once().Text()
                .Or(Parse.LineTerminator);

        private static readonly Parser<string> DataItem =
            Parse.AnyChar
                .Except(Parse.Char(':'))
                .Except(DataItemDelimiter)
                //.Except(Parse.LineTerminator)
                .AtLeastOnce()
                .Text();

        private static readonly Parser<(string key, string value)> KeyValuePair =
            from key in DataItem
            from sep in Parse.Char(':')
            from value in DataItem
            select (key, value);

        private static readonly Parser<Passport> Passport =
            from dataItems in KeyValuePair.DelimitedBy(DataItemDelimiter)
            select new Passport(dataItems);

        private static readonly Parser<string> PassportDelimiter = //Parse.LineTerminator.Text().Then(Parse.LineTerminator.Text());
            from t1 in Parse.LineTerminator
            from t2 in Parse.LineTerminator
            select t1 + t2;

        private static readonly Parser<Passport[]> Passports = //Parse.LineTerminator.Text().Then(Parse.LineTerminator.Text());
            from passports in Passport.DelimitedBy(PassportDelimiter)
            select passports.ToArray();

        public static Passport[] ParseBatch(string batch) => Passports.Parse(batch);
    }
}
