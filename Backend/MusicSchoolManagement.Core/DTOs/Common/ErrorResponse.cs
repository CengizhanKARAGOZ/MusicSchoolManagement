namespace MusicSchoolManagement.Core.DTOs.Common;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public string? TraceId { get; set; }
    public DateTime Timestamp { get; set; }

    public ErrorResponse()
    {
        Timestamp = DateTime.UtcNow;
    }

    public ErrorResponse(int statusCode, string message, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
        Timestamp = DateTime.UtcNow;
    }
}