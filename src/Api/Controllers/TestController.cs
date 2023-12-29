using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class TestController(TestService testService) : ControllerBase
    {
        [HttpDelete("Test/DeleteAllTestEntities")]
        public Task DeleteAllTestEntities()
        {
            return testService.DeleteAllTestEntities();
        }
    }
}
