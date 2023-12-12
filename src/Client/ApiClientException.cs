using Microsoft.AspNetCore.Mvc;

namespace Client
{
    public class ApiClientException(ProblemDetails problemDetails) : Exception(problemDetails.ToJsonString())
    {
        public ProblemDetails ProblemDetails { get; set; } = problemDetails;
    }
}
