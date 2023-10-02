namespace jonas.Middleware;

public class CustomResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Details { get; set; }

    public CustomResponse(int statusCode, string message, string details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }
}
