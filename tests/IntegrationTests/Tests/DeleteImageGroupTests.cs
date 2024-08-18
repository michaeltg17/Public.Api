using ApiClient.Extensions;
using Core.Testing.Builders;
using Core.Testing.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class DeleteImageGroupTests : Test
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GivenImageGroup_WhenDelete_IsDeleted(int version)
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await ApiClient.Api.SaveImageGroup(imagePath).To<ImageGroup>();
            var imageGroup2 = await ApiClient.Api.GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);

            //When
            var deleteResponse = await ApiClient.Api.DeleteImageGroup(imageGroup.Id, version);

            //Then
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponse = await ApiClient.Api.GetImageGroup(imageGroup.Id);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task WhenDeleteNonexistentImageGroup_ExpectedProblemDetails()
        {
            //When
            var response = await ApiClient.Api.DeleteImageGroup(id: 600);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithNotFoundException("/ImageGroup/600", "ImageGroup", 600)
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
