using System;
using AoC.BootCode;
using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests.BootCode
{
    public class BootCodeComputerTests
    {
        [Test]
        public void Day8Part1ExampleTest()
        {
            var sut = BootCodeComputer.Parse(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6");

            Action act = () => sut.Evaluate();

            // ACT
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Infinite loop detected");

            // ASSERT
            sut.Accumulator.Should().Be(5);
        }
    }
}
