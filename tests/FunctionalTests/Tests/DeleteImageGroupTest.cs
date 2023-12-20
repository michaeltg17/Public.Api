using Client;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
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
            var imageGroup = await apiClient.SaveImageGroup(imagePath).To<ImageGroup>();
            var imageGroup2 = await apiClient.GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);

            //When
            var deleteResponse = await apiClient.DeleteImageGroup(imageGroup.Id);

            //Then
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponse = await apiClient.GetImageGroup(imageGroup.Id);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenUnexistingImageGroup_WhenDeleteImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await apiClient.DeleteImageGroup(id: 600);

            //Then
            var expected = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "NotFoundException",
                Status = (int)HttpStatusCode.NotFound,
                Detail = "ImageGroup with id '600' was not found.",
                Instance = "/ImageGroup/600"
            };

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenBadRequest_WhenDeleteImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await apiClient.DeleteImageGroup(id: 600);

            //Then
            dynamic errors = new ExpandoObject();
            errors.id = new[] { "The file field is required." };
            var expected = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "ValidationException",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = "Please check the errors property for additional details.",
                Instance = "/ImageGroup",
                Extensions = new Dictionary<string, object?>() { { "errors", errors } }
            };

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
