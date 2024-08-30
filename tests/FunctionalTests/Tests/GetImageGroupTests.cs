using ApiClient.Extensions;
using Core.Testing.Builders;
using Core.Testing.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class GetImageGroupTests : Test
    {
        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task GivenImageGroup_WhenSaveAndGetImageGroup_IsGot(string apiEndpointsName)
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await ApiClient.GetApiEndpoints(apiEndpointsName).SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var response = await ApiClient.GetApiEndpoints(apiEndpointsName).GetImageGroup(imageGroup.Id);
            var imageGroup2 = await response.To<ImageGroup>();

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            imageGroup.Should().BeEquivalentTo(imageGroup2);
        }

        [InlineData(nameof(ApiClient.ControllerApi))]
        [InlineData(nameof(ApiClient.MinimalApi))]
        [Theory]
        public async Task GivenUnexistingImageGroup_WhenGetImageGroup_ExpectedProblemDetails(string apiEndpointsName)
        {
            //Given
            //When
            var response = await ApiClient.GetApiEndpoints(apiEndpointsName).GetImageGroup(id: 600);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithNotFoundException("/ImageGroup/600", "ImageGroup", 600)
                .Build();

            (await response.To<ProblemDetails>()).Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
