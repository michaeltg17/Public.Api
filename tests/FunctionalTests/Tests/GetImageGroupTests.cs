using ApiClient.Extensions;
using Common.Testing.Builders;
using Common.Testing.Models;
using FluentAssertions;
using FunctionalTests.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class GetImageGroupTests(ITestSettings settings) : Test(settings)
    {
        [Fact]
        public async Task GivenImageGroup_WhenSaveAndGetImageGroup_IsGot()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await apiClient.Api.SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var response = await apiClient.Api.GetImageGroup(imageGroup.Id);
            var imageGroup2 = await response.To<ImageGroup>();

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            imageGroup.Should().BeEquivalentTo(imageGroup2);
        }

        [Fact]
        public async Task GivenUnexistingImageGroup_WhenGetImageGroup_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await apiClient.Api.GetImageGroup(id: 600);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithNotFoundException("/ImageGroup/600", "ImageGroup", 600)
                .Build();

            (await response.To<ProblemDetails>()).Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
