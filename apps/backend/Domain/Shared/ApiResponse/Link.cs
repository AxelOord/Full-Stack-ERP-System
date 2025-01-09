namespace Domain.Shared.ApiResponse
{
    public class Link(string href, string rel, string method)
    {
        public string Href { get; set; } = href;
        public string Rel { get; set; } = rel;
        public string Method { get; set; } = method;
    }
}
