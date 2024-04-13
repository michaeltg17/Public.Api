using ApiClient.Extensions;
using Common.Testing.Builders;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class DeleteImageGroupTests(ISettings settings) : Test(settings)
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
            var expected = new ProblemDetailsBuilder()
                .WithNotFoundException("/ImageGroup/600", "ImageGroup", 600)
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenBadRequest_WhenDeleteImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await apiClient.DeleteImageGroup("blabla");

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithValidationException("/ImageGroup/blabla")
                .WithError("id", "The value 'blabla' is not valid.")
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
