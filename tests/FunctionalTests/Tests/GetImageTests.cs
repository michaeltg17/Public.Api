﻿using ApiClient.Extensions;
using Common.Testing.Builders;
using Common.Testing.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FunctionalTests.Tests
{
    public class GetImageTests(ISettings settings) : Test(settings)
    {
        [Fact]
        public async Task GivenImageGroup_WhenGetImage_IsGot()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            var imageGroup = await apiClient.Api.SaveImageGroup(imagePath).To<ImageGroup>();

            //When
            var image = await apiClient.Api.GetImage(imageGroup.Images.First().Id).To<Image>();

            //Then
            var uploadedImageBytes = File.ReadAllBytes(imagePath);
            var downloadedImageBytes = await apiClient.HttpClient.GetByteArrayAsync(image!.Url);

            uploadedImageBytes.Should().BeEquivalentTo(downloadedImageBytes);
        }

        [Fact]
        public async Task GivenUnexistingImage_WhenGetImage_ExpectedProblemDetails()
        {
            //Given
            //When
            var response = await apiClient.Api.GetImage(id: 600);

            //Then
            var expected = new ProblemDetailsBuilder()
                .WithNotFoundException("/Image/600", "Image", 600)
                .Build();

            var problemDetails = await response.To<ProblemDetails>();
            problemDetails.Should().BeEquivalentTo(expected);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
