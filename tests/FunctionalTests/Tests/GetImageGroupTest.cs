using FluentAssertions;
using Xunit;

namespace FunctionalTests.Tests
{
    public class GetImageGroupTest(ISettings settings) : Test(settings)
    {
        [Fact]
        public async Task GivenImage_WhenSaveAndGetImageGroup_IsGot()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var imageGroup = await apiClient.SaveImageGroup(imagePath);

            //Then
            var imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id);
            imageGroup.Should().BeEquivalentTo(imageGroup2);
        }
    }
}
