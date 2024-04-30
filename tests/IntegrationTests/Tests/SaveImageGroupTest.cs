using ApiClient.Extensions;
using Common.Testing.Builders;
using Common.Testing.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class SaveImageGroupTest : Test
    {
        [Fact]
        public async Task GivenImage_WhenSaveImageGroup_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var response = await ApiClient.SaveImageGroup(imagePath);
            var imageGroup = await response.To<ImageGroup>();

            //Then
            var imageGroup2 = await ApiClient.GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task GivenBadRequest_WhenSaveImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await ApiClient.SaveImageGroup((HttpContent?)null);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithValidationException("/ImageGroup")
                .WithError("file", "The file field is required.")
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
