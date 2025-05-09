namespace BiblioTar.Entities;

public class ApiResponse
{
    public int StatusCode { get; set; } = 200;
    public string? Message { get; set; } = "Success";
    public object? Data { get; set; }
    public bool Success { get; set; } = true;
    public DateTime RespondTime { get; set; } = DateTime.Now;
}