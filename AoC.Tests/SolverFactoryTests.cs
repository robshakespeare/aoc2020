using FluentAssertions;
using NUnit.Framework;

namespace AoC.Tests
{
    public class SolverFactoryTests
    {
        [Test]
        public void CanCreateFactory_And_CreateASolver()
        {
            var sut = SolverFactory.CreateFactory();

            var solver = sut.TryCreateSolver("0");

            solver!.Should().NotBeNull();
            solver!.DayNumber.Should().Be(0);
        }
    }
}
