using Api.Filters;
using Api.Middlewares;
using CrossCutting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Persistence;
using Serilog;
using System.Text.Json.Serialization;
using Api.Extensions;
using Application;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace Api
{
    /// <summary>
    /// WebApplicationFactory needs Program class not to be static and I wanted to use extension methods.
    /// </summary>
    public static class Startup
    {
        public static void Run(string[] args)
        {
            WebApplication
                .CreateBuilder(args)
                .AddDependencies()
                .Build()
                .Configure()
                .Run();
        }

        static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder)
        {
            //Controllers should be added first
            builder.Services
                .AddControllers()
                .AddJsonOptions(c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder
                .AddSwaggerIfDevelopment()
                .AddSerilog();

            builder.Services
                .AddMainDependencies()
                .AddProblemDetails()
                .AddInvalidModelStateResponseFactory()
                .AddFilters()
                .AddVersioning();
                //.AddSecurity();

            return builder;
        }

        static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddAuthentication();

            services.AddAuthorizationBuilder()
                .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());

            return services;
        }

        static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            return services
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.DefaultApiVersion = new ApiVersion(1);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                })
                .AddMvc()
                .Services;
        }

        static IServiceCollection AddMainDependencies(this IServiceCollection services)
        {
            return services
                .AddApplicationDependencies()
                .AddCrossCuttingDependencies()
                .AddPersistanceDependencies();
        }

        static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                ApplyCommonSerilogConfiguration(context, services, configuration);
                configuration.WriteTo.Console();
            });

            return builder;
        }

        public static void ApplyCommonSerilogConfiguration(
            HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration)
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        }

        static IServiceCollection AddInvalidModelStateResponseFactory(this IServiceCollection services)
        {
            return services.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
            {
                apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
                {
                    var problemDetails = new ValidationProblemDetails(actionContext.ModelState)
                    {
                        Type = apiBehaviorOptions.ClientErrorMapping[400].Link,
                        Title = "ValidationException",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please check the errors property for additional details.",
                        Instance = actionContext.HttpContext.Request.Path
                    };

                    return new BadRequestObjectResult(problemDetails);
                };
            });
        }

        static IServiceCollection AddFilters(this IServiceCollection services)
        {
            return services.AddSingleton<SampleFilter>();
        }

        static WebApplicationBuilder AddSwaggerIfDevelopment(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Public API",
                        Description = "A showcase API"
                    });
                });
            }

            return builder;
        }

        static WebApplication Configure(this WebApplication webApplication)
        {
            //Exception middleware first to catch exceptions
            webApplication.UseExceptionHandler();

            webApplication.MapControllers();

            webApplication
                .AddSwaggerIfDevelopment()
                .AddObjectStorageFeature();

            webApplication
                .UseAuthentication()
                .UseAuthorization()
                .UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
                .UseMiddleware<SampleMiddleware>();

            return webApplication;
        }

        static WebApplication AddSwaggerIfDevelopment(this WebApplication webApplication)
        {
            if (webApplication.Environment.IsDevelopment())
                webApplication.UseSwagger().UseSwaggerUI();

            return webApplication;
        }

        static WebApplication AddObjectStorageFeature(this WebApplication webApplication)
        {
            var settings = webApplication.Services.GetRequiredService<ISettings>();
            webApplication.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = settings.ImagesRequestPath,
                FileProvider = new PhysicalFileProvider(settings.ImagesStoragePath)
            });

            return webApplication;
        }
    }
}
