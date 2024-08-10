using Api.Filters;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Test Api")]
        public Task Get(int id)
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
