using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using MyProject.API.Common;
using System.Text.Json;

namespace MyProject.API.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandled exception occurred");

            var (statusCode, response) = MapExceptionToResponse(exception);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json, cancellationToken);

            return true;
        }

        private static (int statusCode, ApiResponse<object> response) MapExceptionToResponse(Exception exception)
        {
            return exception switch
            {
                // 🔸 FluentValidation
                ValidationException fvEx => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.ErrorResponse(
                        fvEx.Errors.Select(e => e.ErrorMessage).FirstOrDefault()!,
                        StatusCodes.Status400BadRequest
                    )
                ),

                // 🔸 Key not found
                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    ApiResponse<object>.ErrorResponse(exception.Message, StatusCodes.Status404NotFound)
                ),

                // 🔸 Unauthorized
                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    ApiResponse<object>.ErrorResponse(exception.Message, StatusCodes.Status401Unauthorized)
                ),

                // 🔸 Argument exception
                ArgumentException => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.ErrorResponse(exception.Message, StatusCodes.Status400BadRequest)
                ),
                // 🔸 Invalid operation
                InvalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.ErrorResponse(exception.Message, StatusCodes.Status400BadRequest)
                ),

                // 🔸 Default fallback
                _ => (
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An unexpected error occurred. Please try again later.",
                        StatusCodes.Status500InternalServerError
                    )
                )
            };
        }
    }
}
