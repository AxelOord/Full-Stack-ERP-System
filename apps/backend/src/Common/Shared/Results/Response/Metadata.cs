namespace Shared.Results.Response;

public class Metadata
{
    public List<Column>? Columns { get; set; }
    public Dictionary<string, Metadata>? Expandable { get; set; }
}
