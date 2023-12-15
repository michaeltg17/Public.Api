using Client;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class GetImageGroupTest(ISettings settings) : Test(settings)
    {
        [Fact]
        public async Task GivenImageGroup_WhenSaveAndGetImageGroup_IsGot()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await apiClient.SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var response = await apiClient.GetImageGroup(imageGroup.Id);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var imageGroup2 = response.To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);
        }

        [Fact]
        public async Task GivenUnexistingImageGroup_WhenGetImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await apiClient.GetImageGroup(id: 600);

            //Then
            var expected = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "NotFoundException",
                Status = (int)HttpStatusCode.NotFound,
                Detail = "ImageGroup with id '600' was not found."
            };

            response.To<ProblemDetails>().Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
