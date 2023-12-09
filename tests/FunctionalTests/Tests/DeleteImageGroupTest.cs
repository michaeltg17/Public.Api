using FluentAssertions;
using Xunit;

namespace FunctionalTests.Tests
{
    public class DeleteImageGroupTest(ISettings settings) : Test(settings)
    {
        [Fact]
        public async Task GivenImageGroup_WhenDelete_IsDeleted()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await apiClient.SaveImageGroup(imagePath);
            var imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id);
            imageGroup.Should().Be(imageGroup2);

            //When
            await apiClient.DeleteImageGroup(imageGroup.Id);

            //Then
            imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id);
            imageGroup2.Should().BeNull();
        }
    }
}
