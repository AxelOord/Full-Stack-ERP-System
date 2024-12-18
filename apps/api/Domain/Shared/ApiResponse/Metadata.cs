
namespace Domain.Shared.ApiResponse
{
    public class Metadata
    {
        public List<Column>? Columns { get; set; }

        public class Column
        {
            public string? Field { get; set; }
            public string? Key { get; set; }
            public string? Type { get; set; }
            public bool? Sortable { get; set; }
        }
    }
}
