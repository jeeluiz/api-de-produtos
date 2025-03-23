namespace Produto.Application;

using System.Text.Json.Serialization;

public class AppResult
{
    public AppResult()
    {
    }

    public AppResult(string message, bool success = false, AppResultStatus statusCode = AppResultStatus.Sucesso)
    {
        Message = message;
        Success = success;
        StatusCode = statusCode;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    public bool Success { get; set; } = true;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AppResultStatus StatusCode { get; set; } = AppResultStatus.Sucesso;
}

public class AppResult<T> : AppResult
{
    public AppResult(T data)
        : base()
    {
        Data = data;
    }

    public AppResult(string message, bool success = false, AppResultStatus statusCode = AppResultStatus.Erro)
        : base(message, success, statusCode)
    { }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }
}