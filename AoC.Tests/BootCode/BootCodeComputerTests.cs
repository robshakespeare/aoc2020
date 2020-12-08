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
            // ACT
            var result = sut.Evaluate();

            // ASSERT
            result.Should().Be(5);
            sut.Accumulator.Should().Be(5);
        }
    }
}
