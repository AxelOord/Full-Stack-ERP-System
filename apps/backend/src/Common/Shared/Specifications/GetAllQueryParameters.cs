using Shared.Specifications.Filtering;

public class GetAllQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? FilterProperty { get; set; }
    public string? FilterValue { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; } = false;

    // Advanced filtering
    public FilterDefinition FilterDefinition { get; set; } = new();
    public Dictionary<string, string> RawFilters { get; set; } = new();
}