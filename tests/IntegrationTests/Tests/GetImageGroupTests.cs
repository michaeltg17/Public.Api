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
    public class GetImageGroupTests : Test
    {
        [Fact]
        public async Task GivenImageGroup_WhenSaveAndGetImageGroup_IsGot()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await ApiClient.SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var response = await ApiClient.GetImageGroup(imageGroup.Id);
            var imageGroup2 = await response.To<ImageGroup>();

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            imageGroup.Images.Should().HaveCount(3);
        }

        [Fact]
        public async Task GivenUnexistingImageGroup_WhenGetImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await ApiClient.GetImageGroup(id: 600);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithNotFoundException("/ImageGroup/600", "ImageGroup", 600)
                .Build();

            (await response.To<ProblemDetails>()).Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenBadRequest_WhenGetImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await ApiClient.GetImageGroup("blabla");

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithValidationException("/ImageGroup/blabla")
                .WithError("id", "The value 'blabla' is not valid.")
                .Build();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
        }
    }
}
