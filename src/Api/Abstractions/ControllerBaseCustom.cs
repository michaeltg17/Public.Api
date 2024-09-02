using Microsoft.AspNetCore.Mvc;

namespace Api.Abstractions
{
    [Route("api/v{version:apiVersion}/{ApiName}/[controller]")]
    public class ControllerBaseCustom : ControllerBase
    {
        const string ApiName = "ControllerApi";
        
    }
}
