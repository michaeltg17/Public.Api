using Api.Filters;
using Api.Models.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Api.Controllers
{
    [Route($"{ApiType}")]
    public class TestController(TestService testService) : ControllerBase
    {
        const string ApiType = "TestControllerApi";
        const string NamePrefix = ApiType + ".";

        [ServiceFilter<SampleFilter>]
        [HttpGet(nameof(GetOk), Name = NamePrefix + nameof(GetOk))]
        public Task GetOk()
        {
            return Task.CompletedTask;
        }

        [HttpGet("Get/{id}", Name = NamePrefix + nameof(Get))]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Test Api")]
        public Task Get(long id)
        {
            return Task.CompletedTask;
        }

        [HttpPost("Post/{id}", Name = NamePrefix + nameof(Post))]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Test Api")]
        public Task Post(long id, [FromQuery] string name, [FromQuery] DateTime date, [FromBody] TestPostRequest request)
        {
            return Task.CompletedTask;
        }

        [HttpPost(nameof(ThrowInternalServerError), Name = NamePrefix + nameof(ThrowInternalServerError))]
        public Task ThrowInternalServerError()
        {
            throw new Exception("Sensitive data");
        }

        [HttpDelete(nameof(DeleteAllTestEntities), Name = NamePrefix + nameof(DeleteAllTestEntities))]
        public Task DeleteAllTestEntities()
        {
            return testService.DeleteAllTestEntities();
        }
    }
}
