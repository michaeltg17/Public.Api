using FluentAssertions;
using FunctionalTests.Others;
using Xunit;

namespace FunctionalTests
{
    public class DeleteImageGroupTest : ApiTest
    {
        [Fact]
        public async Task GivenImageGroup_WhenDelete_IsDeleted()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await Client.SaveImageGroup(imagePath);
            var imageGroup2 = await Client.GetImageGroup(imageGroup.Id);
            imageGroup.Should().Be(imageGroup2);

            //When
            await Client.DeleteImageGroup(imageGroup.Id);

            //Then
            imageGroup2 = await Client.GetImageGroup(imageGroup.Id);
            imageGroup2.Should().BeNull();
        }
    }
}
