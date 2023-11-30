using FluentAssertions;
using FunctionalTests.Others;
using Xunit;

namespace FunctionalTests
{
    public class SaveImageGroupTest : ApiTest
    {
        [Fact]
        public async Task GivenImage_WhenSave_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var imageGroup = await Client.SaveImageGroup(imagePath);

            //Then
            var imageGroup2 = await Client.GetImageGroup(imageGroup.Id);
            imageGroup.Should().Be(imageGroup2);
        }
    }
}
