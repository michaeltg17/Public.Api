using FluentAssertions;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    [Collection(nameof(ApiCollection))]
    public class DeleteAllTestEntitiesTests(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory) 
        : Test(testOutputHelper, factory)
    {
        //[Fact]
        public async Task GivenTestEntitiesInDb_WhenDeleteAllTestEntities_EntitiesAreDeleted()
        {
            //Given
            const string imagePath = @"Images\didi.jpeg";
            await apiClient.SaveImageGroup(imagePath);

            //When
            var deleteResponse = await apiClient.DeleteAllTestEntities();

            //Then
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            //db.ImageGroups.Any(i => i.IsTest == true).Should().BeFalse();
        }
    }
}
