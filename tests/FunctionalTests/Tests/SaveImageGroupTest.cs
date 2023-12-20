using Client;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class SaveImageGroupTest(ISettings settings) : Test(settings)
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
            var response = await apiClient.SaveImageGroup((HttpContent?)null);

            //Then
            dynamic errors = new ExpandoObject();
            errors.file = new[] { "The file field is required." };
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
