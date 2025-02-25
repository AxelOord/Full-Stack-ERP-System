namespace Domain.Specifications;

public class GetAllQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? FilterProperty { get; set; }
    public string? FilterValue { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; } = false;
}
