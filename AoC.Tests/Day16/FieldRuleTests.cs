using System;
using AoC.Day16;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.Day16
{
    public class FieldRuleTests
    {
        [Test]
        public void FieldRule_IsValidValue_Tests()
        {
            var sut = new FieldRule("test", new[]
            {
                new Range(1, 3),
                new Range(5, 7)
            });

            // ASSERT
            sut.IsValidValue(1).Should().BeTrue();
            sut.IsValidValue(2).Should().BeTrue();
            sut.IsValidValue(3).Should().BeTrue();

            sut.IsValidValue(5).Should().BeTrue();
            sut.IsValidValue(6).Should().BeTrue();
            sut.IsValidValue(7).Should().BeTrue();

            sut.IsValidValue(4).Should().BeFalse();
            sut.IsValidValue(0).Should().BeFalse();
            sut.IsValidValue(8).Should().BeFalse();
            sut.IsValidValue(100).Should().BeFalse();
        }
    }
}
