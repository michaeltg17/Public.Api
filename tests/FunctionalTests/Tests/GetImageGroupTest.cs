using Client;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

        [Fact]
        public async Task GivenUnexistingImageGroup_WhenGetImageGroup_ExpectedProblemDetails()
        {
            //When
            var getImage = async () => await apiClient.GetImageGroup(id: 600);

            //Then
            var expected = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "NotFoundException",
                Status = (int)HttpStatusCode.NotFound,
                Detail = "ImageGroup with id '600' was not found."
            };

            (await getImage.Should().ThrowAsync<ApiClientException>())
            .And.ProblemDetails.Should().BeEquivalentTo(expected, options => options.Excluding(p => p.Extensions));
        }
    }
}
