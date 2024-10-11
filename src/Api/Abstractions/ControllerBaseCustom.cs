using Microsoft.AspNetCore.Mvc;

namespace Api.Abstractions
{
    [Route("api/v{version:apiVersion}" + $"/{ApiType}/[controller]")]
    public class ControllerBaseCustom : ControllerBase
    {
        private protected const string ApiType = "ControllerApi";
        private protected const string NamePrefix = ApiType + ".";
    }
}
