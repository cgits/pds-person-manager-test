using System.Net;
using System.Text.Json;
using UKParliament.CodeTest.Models.Exceptions;
using UKParliament.CodeTest.Models.Models;

namespace UKParliament.CodeTest.Web.Middleware;

public class ErrorResponseWrapper
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorResponseWrapper> _logger;

    public ErrorResponseWrapper(RequestDelegate next, ILogger<ErrorResponseWrapper> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, $"There was a validation error when calling {context.Request.Path}");
        
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse(ex)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"There was unhandled error when calling {context.Request.Path}");
        
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse("There was an error attempting the operation, please try again.")));
        }
    }
}