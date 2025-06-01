namespace Business.Models;

public class ApiResponse<T>
{
    public T Result { get; set; } = default!;
    public bool Success { get; set; }
    public string? Error { get; set; }
}