using Api.Filters;
using Api.Models.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Api.Controllers
{
    [Route("TestController")]
    public class TestController(TestService testService) : ControllerBase
    {
        [ServiceFilter<SampleFilter>]
        [HttpGet(nameof(GetOk), Name = nameof(GetOk))]
        public Task GetOk()
        {
            return Task.CompletedTask;
        }

        [HttpGet("Get/{id}", Name = "TestController.Get")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Test Api")]
        public Task Get(long id)
        {
            return Task.CompletedTask;
        }

        [HttpPost("Post/{id}", Name = "TestController.Post")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Test Api")]
        public Task Post(long id, [FromQuery] string name, [FromQuery] DateTime date, [FromBody] TestPostRequest request)
        {
            return Task.CompletedTask;
        }

        [HttpPost(nameof(ThrowInternalServerError), Name = nameof(ThrowInternalServerError))]
        public Task ThrowInternalServerError()
        {
            throw new Exception("Sensitive data");
        }

        [HttpDelete(nameof(DeleteAllTestEntities), Name = nameof(DeleteAllTestEntities))]
        public Task DeleteAllTestEntities()
        {
            return testService.DeleteAllTestEntities();
        }
    }
}
