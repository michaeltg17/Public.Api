//using FluentAssertions;
//using System.Net;
//using Xunit;

//namespace IntegrationTests.Tests
//{
//    [Collection(nameof(ApiCollection))]
//    public class DeleteAllTestEntitiesTests(WebApplicationFactoryFixture factory) : Test( factory)
//    {
//        //[Fact]
//        public async Task GivenTestEntitiesInDb_WhenDeleteAllTestEntities_EntitiesAreDeleted()
//        {
//            //Given
//            const string imagePath = @"Images\didi.jpeg";
//            await apiClient.SaveImageGroup(imagePath);

//            //When
//            var deleteResponse = await apiClient.DeleteAllTestEntities();

//            //Then
//            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
//            //db.ImageGroups.Any(i => i.IsTest == true).Should().BeFalse();
//        }
//    }
//}
