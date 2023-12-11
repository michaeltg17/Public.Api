using FluentAssertions;
using System.Net;
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
            imageGroup.Should().BeEquivalentTo(imageGroup2);

            //When
            await apiClient.DeleteImageGroup(imageGroup.Id);

            //Then
            var getImageGroup = async () => await apiClient.GetImageGroup(imageGroup.Id);
            (await getImageGroup.Should().ThrowAsync<HttpRequestException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
