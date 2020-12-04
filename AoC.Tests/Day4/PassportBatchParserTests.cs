using System.Collections.Generic;
using System.Linq;
using AoC.Day4;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day4
{
    public class PassportBatchParserTests
    {
        public class TheParseBatchMethod
        {
            [Test]
            public void CanParseSinglePassportOnSingleLine()
            {
                // ACT
                var result = PassportBatchParser.ParseBatch("eyr:2024 pid:662406624 hcl:#cfa07d byr:1947 iyr:2015 ecl:amb hgt:150cm").Single();

                // ASSERT
                var expected = new Passport(
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
                var expected = new Passport(
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
                var result = PassportBatchParser.ParseBatch(input);

                // ASSERT
                result.Should().BeEquivalentTo(
                    new Passport(("test", "test1")),
                    new Passport(("test", "test2"), ("hello", "world"), ("3", "three")),
                    new Passport(("something", "value"), ("and", "this")));
            }
        }
    }
}
