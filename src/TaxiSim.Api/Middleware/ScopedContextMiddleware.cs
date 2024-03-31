using TaxiSim.Api.SharedContext;

namespace TaxiSim.Api.Middleware;

public class ScopedContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, IScopedContext scopedContext)
    {
        var valueToSet = Guid.NewGuid();
        scopedContext.SetUserId(Database.Primitives.UserId.From(valueToSet));

        await next(httpContext);
    }
}

public static class ScopedContextMiddlewareExtensions
{
    public static IApplicationBuilder UseScopedContext(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ScopedContextMiddleware>();
    }
}