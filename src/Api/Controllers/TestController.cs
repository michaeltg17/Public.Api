using Api.Filters;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class TestController(TestService testService) : ControllerBase
    {
        [ServiceFilter<SampleFilter>]
        [HttpGet("Test/GetOk")]
        public Task GetOk()
        {
            return Task.CompletedTask;
        }

        [HttpPost("Test/ThrowInternalServerError")]
        public Task ThrowInternalServerError()
        {
            throw new Exception("Sensitive data");
        }

        [HttpDelete("Test/DeleteAllTestEntities")]
        public Task DeleteAllTestEntities()
        {
            return testService.DeleteAllTestEntities();
        }
    }
}
