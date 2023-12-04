using FluentAssertions;
using IntegrationTests.Others;
using Michael.Net.Extensions.DateTimeExtensions;
using Michael.Net.Testing.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    [Collection("ApiCollection")]
    public class SaveImageGroupTest(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory) : Test(testOutputHelper, factory)
    {
        [Fact]
        public async Task GivenImage_WhenSaveImageGroup_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var imageGroup = await apiClient.SaveImageGroup(imagePath);

            //Then
            var imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id);
            //imageGroup.Should().BeEquivalentTo(imageGroup2, o => o
            //    .ExcludingCollectionProperty(i => i.Images.First().ResolutionNavigation));
            imageGroup.Should().BeEquivalentTo(imageGroup2);
        }
    }
}
