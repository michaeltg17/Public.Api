using Api.Filters;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("Test")]
    public class TestController(TestService testService) : ControllerBase
    {
        [ServiceFilter<SampleFilter>]
        [HttpGet(nameof(GetOk), Name = nameof(GetOk))]
        public Task GetOk()
        {
            return Task.CompletedTask;
        }

        [HttpGet("Get/{id:int?}", Name = nameof(Get))]
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
