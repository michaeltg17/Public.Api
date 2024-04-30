using FluentAssertions;
using System.Net;
using Xunit;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class DeleteAllTestEntitiesTests : Test
    {
        //[Fact]
        public async Task GivenTestEntitiesInDb_WhenDeleteAllTestEntities_EntitiesAreDeleted()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            await ApiClient.SaveImageGroup(imagePath);

            //When
            var deleteResponse = await ApiClient.DeleteAllTestEntities();

            //Then
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            //db.ImageGroups.Any(i => i.IsTest == true).Should().BeFalse();
        }
    }
}
