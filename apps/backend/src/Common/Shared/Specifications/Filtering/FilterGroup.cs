namespace Shared.Specifications.Filtering;
public class FilterGroup
{
    public List<FilterCriteria> Criteria { get; set; } = new();
    public List<FilterGroup> Groups { get; set; } = new();
    public string LogicalOperator { get; set; } = "AND";
}