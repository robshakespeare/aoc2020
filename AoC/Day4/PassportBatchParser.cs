using System.Linq;
using Sprache;

namespace AoC.Day4
{
    public static class PassportBatchParser
    {
        private static readonly Parser<string> DataItemDelimiter =
            Parse.Char(' ').Once().Text()
                .Or(Parse.LineTerminator);

        private static readonly Parser<string> DataItem =
            Parse.AnyChar
                .Except(Parse.Char(':'))
                .Except(DataItemDelimiter)
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

        private static readonly Parser<string> PassportDelimiter =
            from t1 in Parse.LineTerminator
            from t2 in Parse.LineTerminator
            select t1 + t2;

        private static readonly Parser<Passport[]> Passports =
            from passports in Passport.DelimitedBy(PassportDelimiter)
            select passports.ToArray();

        public static Passport[] ParseBatch(string batch) => Passports.Parse(batch);
    }
}
