namespace Api.Middlewares
{
    public class ValidationMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(BadHttpRequestException exception)
            {
                _ = exception.InnerException;
                throw;
            }
        }
    }
}
