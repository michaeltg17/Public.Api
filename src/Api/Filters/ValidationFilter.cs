using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace Api.Filters
{
    public class ValidationFilter : IEndpointFilter
    {
        [SuppressMessage("Style", "IDE0042:Deconstruct variable declaration", Justification = "")]
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Dictionary<string, string[]> validationErrors = [];
            foreach (var argument in context.Arguments)
            {
                if (argument == null) continue;

                var validationResult = Validate(argument);

                if (validationResult.IsValid) continue;

                // Group error messages by member name
                var validationResults = validationResult.Details
                    .SelectMany(vr => vr.MemberNames.Select(member => new { member, vr.ErrorMessage }))
                    .GroupBy(item => item.member)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(item => item.ErrorMessage).ToArray()
                    );

                // Merge with the existing dictionary
                foreach (var kvp in validationResults)
                {
                    if (validationErrors.TryGetValue(kvp.Key, out string[]? value))
                    {
                        validationErrors[kvp.Key] = [.. value, .. kvp.Value];
                    }
                    else
                    {
                        validationErrors[kvp.Key] = kvp.Value.Select(s => s ?? "Null error message detected").ToArray();
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                var problemDetails = new ValidationProblemDetails(validationErrors)
                {
                    //Type = options.ClientErrorMapping[400].Link,
                    Title = "ValidationException",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = "Please check the errors property for additional details.",
                    Instance = context.HttpContext.Request.Path
                };

                return Results.ValidationProblem(validationErrors);
            };

            return await next(context);
        }

        public static (List<ValidationResult> Details, bool IsValid) Validate(object @object)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(@object);

            var isValid = Validator.TryValidateObject(@object, context, results, true);

            return (results, isValid);
        }
    }
}
