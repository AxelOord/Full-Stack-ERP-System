using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Domain.Shared.ApiResponse
{
    public class LinkBuilder(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        private readonly LinkGenerator _linkGenerator = linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        // TODO: should be a global setting or something
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;

        public ApiLinks CreatePaginationLinks(string routeName, HttpRequest request, int resultCount)
        {
            ArgumentNullException.ThrowIfNull(routeName);
            ArgumentNullException.ThrowIfNull(request);

            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("No current HttpContext is available.");

            int currentPage = ParseQueryParam(request, "pageNumber", DefaultPageNumber);
            int pageSize = ParseQueryParam(request, "pageSize", DefaultPageSize);

            // Optional filters
            string? filterProperty = ParseQueryParam<string>(request, "FilterProperty", null);
            string? filterValue = ParseQueryParam<string>(request, "FilterValue", null);

            // Create route parameters
            var routeParams = new Dictionary<string, object>
            {
                { "pageNumber", currentPage },
                { "pageSize", pageSize }
            };

            // Add filters if present
            if (!string.IsNullOrWhiteSpace(filterProperty))
            {
                routeParams["FilterProperty"] = filterProperty;
            }

            if (!string.IsNullOrWhiteSpace(filterValue))
            {
                routeParams["FilterValue"] = filterValue;
            }

            // Generate self link
            var self = $"{request.Scheme}://{request.Host}{routeName}?{GenerateQueryString(routeParams)}";

            // Generate next link
            routeParams["pageNumber"] = currentPage + 1;
            var next = resultCount >= pageSize
                ? $"{request.Scheme}://{request.Host}{routeName}?{GenerateQueryString(routeParams)}"
                : null;

            // Generate previous link
            routeParams["pageNumber"] = currentPage - 1;
            var prev = (currentPage - 1) <= 0
                ? null
                : $"{request.Scheme}://{request.Host}{routeName}?{GenerateQueryString(routeParams)}";


            return new ApiLinks
            {
                Self = self,
                Next = next,
                Prev = prev
            };
        }

        public ApiLinks CreateDtoLinks(HttpRequest request, string type, Guid id)
        {
            // FIXME: doesnt work
            var self = $"{request.Scheme}://{request.Host}{request.Path}/{id}";

            return new ApiLinks
            {
                Self = self
            };
        }

        private static string GenerateQueryString(Dictionary<string, object> parameters)
        {
            return string.Join("&", parameters
                .Where(p => p.Value != null)
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString()!)}"));
        }

        private static T? ParseQueryParam<T>(HttpRequest request, string key, T? defaultValue = default)
        {
            if (request.Query.TryGetValue(key, out var valueString))
            {
                if (typeof(T) == typeof(int) && int.TryParse(valueString, out int parsedInt) && parsedInt > 0)
                {
                    return (T)(object)parsedInt;
                }
                else if (typeof(T) == typeof(string) && !string.IsNullOrWhiteSpace(valueString))
                {
                    return (T)(object)valueString.ToString();
                }
            }

            if (defaultValue is not null)
            {
                return defaultValue;
            }

            return default;
        }
    }
}
