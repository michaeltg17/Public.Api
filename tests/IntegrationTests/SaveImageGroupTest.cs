using Domain.Models;
using FluentAssertions;
using FluentAssertions.Equivalency;
using IntegrationTests.Others;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    [Collection("ApiCollection")]
    public class SaveImageGroupTest : Test
    {
        public SaveImageGroupTest(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory) : base(testOutputHelper, factory) { }

        [Fact]
        public async Task GivenImage_WhenSaveImageGroup_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var imageGroup = await apiClient.SaveImageGroup(imagePath);

            //Then
            var imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id);
            imageGroup.Should().BeEquivalentTo(imageGroup2, o => o
                .Excluding((IMemberInfo m) => m.Path.EndsWith(nameof(Image.ResolutionNavigation)))
                .Using<DateTime>(c => c.Subject.Should().BeCloseTo(c.Expectation, TimeSpan.FromSeconds(2)))
                .When(info => info.Path.EndsWith(nameof(ImageGroup.CreatedOn))));
        }
    }
}
