using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class TestController(TestService testService) : ControllerBase
    {
        [HttpPost("Test/InternalServerError")]
        public Task InternalServerError()
        {
            throw new Exception("You should not see this.");
        }

        [HttpDelete("Test/DeleteAllTestEntities")]
        public Task DeleteAllTestEntities()
        {
            return testService.DeleteAllTestEntities();
        }
    }
}
