using System.Linq;
using AoC.Day4;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day4
{
    public class PassportBatchParserTests
    {
        private static Passport NewPassport(params (string key, string value)[] dataItems) => new(dataItems.ToDictionary(kvp => kvp.key, kvp => kvp.value));

        public class TheParseBatchMethod
        {
            [Test]
            public void CanParseSinglePassportOnSingleLine()
            {
                // ACT
                var result = PassportBatchParser.ParseBatch("eyr:2024 pid:662406624 hcl:#cfa07d byr:1947 iyr:2015 ecl:amb hgt:150cm").Single();

                // ASSERT
                var expected = NewPassport(
                    ("eyr", "2024"), ("pid", "662406624"), ("hcl", "#cfa07d"), ("byr", "1947"), ("iyr", "2015"), ("ecl", "amb"), ("hgt", "150cm"));

                result.Should().BeEquivalentTo(expected);
            }

            [Test]
            public void CanParseSinglePassportAcrossMultipleLines()
            {
                // ACT
                var result = PassportBatchParser.ParseBatch(
                    @"eyr:2024 pid:662406624 hcl:#cfa07d byr:1947
iyr:2015 ecl:amb
hgt:150cm").Single();

                // ASSERT
                var expected = NewPassport(
                    ("eyr", "2024"), ("pid", "662406624"), ("hcl", "#cfa07d"), ("byr", "1947"), ("iyr", "2015"), ("ecl", "amb"), ("hgt", "150cm"));

                result.Should().BeEquivalentTo(expected);
            }

            [Test]
            public void CanParseMultiplePassports()
            {
                const string input = @"test:test1

test:test2 hello:world
3:three

something:value
and:this";

                // ACT
                var result = PassportBatchParser.ParseBatch(input).ToArray();

                // ASSERT
                result.Should().BeEquivalentTo(
                    NewPassport(("test", "test1")),
                    NewPassport(("test", "test2"), ("hello", "world"), ("3", "three")),
                    NewPassport(("something", "value"), ("and", "this")));

                result.Should().OnlyContain(x => x.DataItems.Count > 0);
            }
        }
    }
}
