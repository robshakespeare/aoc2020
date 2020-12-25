using System;
using System.IO;
using AoC.Day8;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.BootCode
{
    public class BootCodeProgramTests
    {
        [Test]
        public void Day8Part1ExampleTest()
        {
            var sut = BootCodeProgram.Parse(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6");

            Action act = () => sut.Evaluate();

            // ACT & ASSERT
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Infinite loop detected");

            sut.Accumulator.Should().Be(5);
        }

        [Test]
        public void Day8Part2WithCorruptionFixedTest()
        {
            var inputPath = Path.Combine("BootCode", "Day8-input-corruption-fixed.txt");
            var sut = BootCodeProgram.Parse(File.ReadAllText(inputPath));

            // ACT
            var result = sut.Evaluate();

            // ASSERT
            result.Should().Be(1125);
            sut.Accumulator.Should().Be(1125);
        }
    }
}
