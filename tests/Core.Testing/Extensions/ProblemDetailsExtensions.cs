using Microsoft.AspNetCore.Mvc;

namespace Core.Testing.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static string GetTraceId(this ProblemDetails problemDetails) => (string)problemDetails.Extensions["traceId"]!;
    }
}
