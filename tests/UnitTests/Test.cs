using FluentAssertions;
using Xunit;

namespace UnitTests
{
    public class Test
    {
        [Fact]
        public void TestMethod()
        {
            (1 + 1).Should().Be(2);
        }
    }
}