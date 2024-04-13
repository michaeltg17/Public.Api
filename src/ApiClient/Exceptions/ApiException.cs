using ApiClient.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiClient.Exceptions
{
    public class ApiException(ProblemDetails problemDetails) : ApiClientException(problemDetails.ToJsonString())
    {
        public ProblemDetails ProblemDetails { get; set; } = problemDetails;
    }
}
