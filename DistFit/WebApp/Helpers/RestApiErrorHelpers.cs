using System.Diagnostics;
using System.Net;
using WebApp.DTO;

namespace WebApp.Helpers;

/// <summary>
/// Helper methods for most used error response formats
/// </summary>
public static class RestApiErrorHelpers
{
    /// <summary>
    /// 400 Bad Request error response
    /// </summary>
    /// <param name="traceId">Trace ID</param>
    /// <returns>Formatted Bad Request error response</returns>
    public static  RestApiErrorResponse GetBadRequestErrorResponse(string traceId) => 
        GetErrorResponse("6.5.1", "Bad request", HttpStatusCode.BadRequest, traceId);
    /// <summary>
    /// 404 Not Found error response
    /// </summary>
    /// <param name="traceId">Trace ID</param>
    /// <returns>Formatted Not Found error response</returns>
    public static  RestApiErrorResponse GetNotFoundErrorResponse(string traceId) => 
        GetErrorResponse("6.5.4", "Not found", HttpStatusCode.NotFound, traceId);
    /// <summary>
    /// 500 Server Error error response
    /// </summary>
    /// <param name="traceId">Trace ID</param>
    /// <returns>Formatted Server Error error response</returns>
    public static  RestApiErrorResponse GetServerErrorResponse(string traceId) => 
        GetErrorResponse("6.6.1", "Server error", HttpStatusCode.InternalServerError, traceId);

    private static RestApiErrorResponse GetErrorResponse(
        string ietfSectionNumber, 
        string responseTitle,
        HttpStatusCode statusCode,
        string traceId)
    {
        return new RestApiErrorResponse
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-" + ietfSectionNumber,
            Title = responseTitle,
            Status = statusCode,
            TraceId = Activity.Current?.Id ?? traceId
        };
    }
}