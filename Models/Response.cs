namespace SWGEmuModManagerApi.Models;

public class Response<T>
{
    public Response()
    {

    }

    public Response(bool succeeded, string message, string[] errors, T? data)
    {
        Succeeded = succeeded;
        Message = message;
        Errors = errors;
        Data = data;
    }

    public Response(T? data)
    {
        Succeeded = true;
        Message = string.Empty;
        Errors = null;
        Data = data;
    }

    public T? Data { get; set; }
    public bool Succeeded { get; set; }
    public string[]? Errors { get; set; }
    public string? Message { get; set; }
}
