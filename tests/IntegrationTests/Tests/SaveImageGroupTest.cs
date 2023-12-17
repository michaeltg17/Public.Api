using Client;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
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
            var response = await apiClient.SaveImageGroup(imagePath);
            var imageGroup = await response.To<ImageGroup>();

            //Then
            var imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task GivenBadRequest_WhenSaveImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await apiClient.HttpClient.PostAsync("ImageGroup", null);

            //Then
            var expected = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "NotFoundException",
                Status = (int)HttpStatusCode.NotFound,
                Detail = "Image with id '600' was not found."
            };

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
