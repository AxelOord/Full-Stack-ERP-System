namespace Shared.Specifications.Filtering;
public class FilterCriteria
{
    public string Path { get; set; }
    public string Operator { get; set; } = FilterOperator.Equals;
    public object Value { get; set; }

    public FilterCriteria() { }

    public FilterCriteria(string path, string @operator, object value)
    {
        Path = path;
        Operator = @operator;
        Value = value;
    }
}