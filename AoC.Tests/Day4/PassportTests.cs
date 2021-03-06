using System.Linq;
using AoC.Day4;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day4
{
    public class PassportTests
    {
        public class TheParseBatchMethod
        {
            private static Passport NewPassport(params (string key, string value)[] dataItems) => new(dataItems);

            [Test]
            public void CanParseSinglePassportOnSingleLine()
            {
                // ACT
                var result = Passport.ParseBatch("eyr:2024 pid:662406624 hcl:#cfa07d byr:1947 iyr:2015 ecl:amb hgt:150cm").Single();

                // ASSERT
                var expected = NewPassport(
                    ("eyr", "2024"), ("pid", "662406624"), ("hcl", "#cfa07d"), ("byr", "1947"), ("iyr", "2015"), ("ecl", "amb"), ("hgt", "150cm"));

                result.Should().BeEquivalentTo(expected);
            }

            [Test]
            public void CanParseSinglePassportAcrossMultipleLines()
            {
                // ACT
                var result = Passport.ParseBatch(
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
                var result = Passport.ParseBatch(input).ToArray();

                // ASSERT
                result.Should().BeEquivalentTo(
                    NewPassport(("test", "test1")),
                    NewPassport(("test", "test2"), ("hello", "world"), ("3", "three")),
                    NewPassport(("something", "value"), ("and", "this")));

                result.Should().OnlyContain(x => x.DataItems.Count > 0);
            }
        }

        public class TheIsValidProperty
        {
            [Test]
            public void IsValid_Should_ReturnFalse_ForInvalidPassports()
            {
                var passports = Passport.ParseBatch(@"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007");

                // ACT & ASSERT
                passports.Should().OnlyContain(passport => !passport.IsValid);
            }

            [Test]
            public void IsValid_Should_ReturnTrue_ForValidPassports()
            {
                var passports = Passport.ParseBatch(@"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719");

                // ACT & ASSERT
                passports.Should().OnlyContain(passport => passport.IsValid);
            }
        }
    }
}
