using MyProject.API.Middlewares;
using MyProject.Application.Interface;

namespace MyProject.API.Middlewares
{
    public class TokenBlackListMiddleware(RequestDelegate _next, ILogger<TokenBlackListMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext context, IRedisService redisService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                var isBlackListed = await redisService.IsTokenBlackListAsync(token);
                if (isBlackListed)
                {
                    _logger.LogWarning("Blacklisted token attempt: {Token}", token);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        code = 401,
                        status = "Unauthorized",
                        message = "Token has been revoked. Please login again.",
                        data = (object?)null
                    });

                    return; // Dừng pipeline
                }
            }
            await _next(context);

        }
    }
}

/// <summary>
/// token blacklist middleware extensions
/// </summary>
public static class TokenBlackListMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenBlackList(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenBlackListMiddleware>();
    }
}
