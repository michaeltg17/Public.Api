﻿using Core.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net;

namespace Core.Testing.Builders
{
    public class ProblemDetailsBuilder : BuilderWithNew<ProblemDetailsBuilder, ProblemDetails>
    {
        public ProblemDetailsBuilder() { Item.Extensions = new Dictionary<string, object?>(); }

        public ProblemDetailsBuilder WithTraceId(string traceId)
        {
            Item.Extensions.Add("traceId", traceId);
            return this;
        }

        public ProblemDetailsBuilder WithNotFound()
        {
            Item.Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
            Item.Title = "Not Found";
            Item.Status = (int)HttpStatusCode.NotFound;

            return this;
        }

        public ProblemDetailsBuilder WithNotFoundException(string apiType, string entity, long id)
        {
            Item.Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
            Item.Title = "NotFoundException";
            Item.Status = (int)HttpStatusCode.NotFound;
            Item.Detail = $"{entity} with id '{id}' was not found.";
            Item.Instance = $"/api/v1/{apiType}/{entity}/{id}";

            return this;
        }

        public ProblemDetailsBuilder WithValidationException(string instance)
        {
            Item.Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
            Item.Title = "ValidationException";
            Item.Status = (int)HttpStatusCode.BadRequest;
            Item.Detail = "Please check the errors property for additional details.";
            Item.Instance = instance;
            Item.Extensions.Add("errors", null);

            return this;
        }

        public ProblemDetailsBuilder WithInternalServerError(string instance)
        {
            Item.Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1";
            Item.Title = "InternalServerError";
            Item.Status = (int)HttpStatusCode.InternalServerError;
            Item.Detail = "Internal server error. Please contact the API support.";
            Item.Instance = instance;

            return this;
        }

        public ProblemDetailsBuilder WithError(string property, string error)
        {
            if (Item.Extensions["errors"] is not ExpandoObject) Item.Extensions["errors"] = new ExpandoObject();

            var dictionary = (IDictionary<string, object?>)Item.Extensions["errors"]!;
            dictionary.Add(property, new[] { error });

            return this;
        }
    }
}
