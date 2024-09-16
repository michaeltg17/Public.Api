using Application.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Application.Services
{
    public class MyServiceTests
    {
        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void WhenMocked_ShouldMockGet1MethodButUseRealGet2Method(bool doMock)
        {
            //Given
            const string mockedValue = "5";
            var myServiceMock = new Mock<MyService>(){ CallBase = true }.As<IMyService>();
            if (doMock) myServiceMock.Setup(s => s.Get1()).Returns(mockedValue);

            //When
            var result = myServiceMock.Object.Get1();

            //Then
            var expected = doMock ? mockedValue : "1";
            result.Should().Be(expected);
            myServiceMock.Object.Get2().Should().Be("2");
        }
    }
}
