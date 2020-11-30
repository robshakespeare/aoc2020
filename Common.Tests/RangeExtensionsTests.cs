using FluentAssertions;
using NUnit.Framework;

namespace Common.Tests
{
    public class RangeExtensionsTests
    {
        [Test]
        public void ToEnumerable_Test()
        {
            (5..10).ToEnumerable().Should().BeEquivalentTo(5, 6, 7, 8, 9);
        }

        [Test]
        public void ToArray_Test()
        {
            (0..4).ToArray().Should().BeEquivalentTo(0, 1, 2, 3);
        }
    }
}
