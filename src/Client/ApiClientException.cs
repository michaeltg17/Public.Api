using Microsoft.AspNetCore.Mvc;

namespace Client
{
    public class ApiClientException(ProblemDetails problemDetails) : Exception(ProblemDetailsExtensions.ToString(problemDetails))
    {
        public ProblemDetails ProblemDetails { get; set; } = problemDetails;
    }
}
