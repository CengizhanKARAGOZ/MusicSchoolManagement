using MusicSchoolManagement.API.Middleware;

namespace MusicSchoolManagement.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}