namespace Produto.Api;

internal static class Extensions
{
    public static IReadOnlyDictionary<string, string> ParseSortBy(
        this string? sortBy,
        char separator = ',',
        char separator2 = ':',
        IReadOnlyCollection<string>? allowedSortBy = null
        )
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return new Dictionary<string, string>();

        var sortByList = sortBy.Split(separator: separator,
                                      options: StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var result = new Dictionary<string, string>(sortByList.Length);

        List<string> alreadyAdded = new(sortByList.Length);

        foreach (var item in sortByList) {
            var parts = item.Split(separator: separator2,
                                   options: StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var lowerCase = parts[0].ToLowerInvariant();
            if (alreadyAdded.Contains(lowerCase))
                continue;
            alreadyAdded.Add(lowerCase);
            if (parts.Length == 2 && (parts[1] == "asc" || parts[1] == "desc"))
                result.Add(parts[0], parts[1]);
            else
                result.Add(parts[0], "asc");
        }

        return result.AsReadOnly();
    }
}