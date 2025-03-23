namespace Produto.Application;

public class AppPagedResult<T> : AppResult<IReadOnlyCollection<T>>
{
    public AppPagedResult(IReadOnlyCollection<T> data, int totalCount, int page, int pageSize)
        : base(data)
    {
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
    public AppPagedResult(string message, bool success = false, AppResultStatus statusCode = AppResultStatus.Erro)
        : base(message, success, statusCode)
    {
    }

    public int? TotalCount { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }

}