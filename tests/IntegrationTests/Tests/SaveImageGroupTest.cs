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
    public class SaveImageGroupTest : Test
    {
        [Fact]
        public async Task GivenImage_WhenSaveImageGroup_IsSaved()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";

            //When
            var response = await ApiClient.Api.SaveImageGroup(imagePath);
            var imageGroup = await response.To<ImageGroup>();

            //Then
            var imageGroup2 = await ApiClient.Api.GetImageGroup(imageGroup.Id).To<ImageGroup>();
            imageGroup.Should().BeEquivalentTo(imageGroup2);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
