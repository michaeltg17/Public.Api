using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace WebStartup.Middleware;
public class TestStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            builder.UseHttpLogging();
            next(builder);
        };
    }
}