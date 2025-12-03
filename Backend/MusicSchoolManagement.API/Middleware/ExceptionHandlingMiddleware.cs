using System.Net;
using System.Text.Json;
using MusicSchoolManagement.Core.Exceptions;

namespace MusicSchoolManagement.API.Middleware;

public class ExceptionHandlingMiddleware
{
    #region Fields

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    #endregion

    #region Constructor

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    #endregion

    #region Public Methods

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    #endregion

    #region Private Methods

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            NotFoundException notFoundException => new ErrorResponse(
                (int)HttpStatusCode.NotFound,
                "Resource Not Found",
                notFoundException.Message),

            BadRequestException badRequestException => new ErrorResponse(
                (int)HttpStatusCode.BadRequest,
                "Bad Request",
                badRequestException.Message),

            ValidationException validationException => new ErrorResponse(
                (int)HttpStatusCode.BadRequest,
                "Validation Failed",
                validationException.Message)
            {
                Errors = validationException.Errors
            },

            UnauthorizedException unauthorizedException => new ErrorResponse(
                (int)HttpStatusCode.Unauthorized,
                "Unauthorized",
                unauthorizedException.Message),

            ForbiddenException forbiddenException => new ErrorResponse(
                (int)HttpStatusCode.Forbidden,
                "Forbidden",
                forbiddenException.Message),

            ConflictException conflictException => new ErrorResponse(
                (int)HttpStatusCode.Conflict,
                "Conflict",
                conflictException.Message),

            InvalidOperationException invalidOperationException => new ErrorResponse(
                (int)HttpStatusCode.BadRequest,
                "Invalid Operation",
                invalidOperationException.Message),

            _ => new ErrorResponse(
                (int)HttpStatusCode.InternalServerError,
                "Internal Server Error",
                _env.IsDevelopment() ? exception.Message : "An error occurred while processing your request.")
        };

        errorResponse.TraceId = context.TraceIdentifier;

        if (_env.IsDevelopment() && exception is not NotFoundException 
                                  && exception is not BadRequestException
                                  && exception is not ValidationException)
        {
            errorResponse.Details = exception.StackTrace;
        }

        context.Response.StatusCode = errorResponse.StatusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(errorResponse, options);
        await context.Response.WriteAsync(json);
    }

    #endregion
}