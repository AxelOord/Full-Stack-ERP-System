namespace Domain.Shared.ApiResponse
{
    public class Column
    {
        public required string Field { get; set; }
        public required string Label { get; set; }
        public required string Type { get; set; }
        public required bool Sortable { get; set; }
    }
}
