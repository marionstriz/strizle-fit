using System.Net;

namespace WebApp.DTO;

/// <summary>
/// Error response format for REST API
/// </summary>
public class RestApiErrorResponse
{
    /// <summary>
    /// Error type (link to documentation)
    /// </summary>
    public string Type { get; set; } = default!;
    /// <summary>
    /// Error title
    /// </summary>
    public string Title { get; set; } = default!;
    /// <summary>
    /// Error HTTP status code
    /// </summary>
    public HttpStatusCode Status { get; set; }
    /// <summary>
    /// Error trace ID
    /// </summary>
    public string TraceId { get; set; } = default!;
    /// <summary>
    /// List of errors according to title
    /// </summary>
    public Dictionary<string, List<string>> Errors { get; set; } = new();
}