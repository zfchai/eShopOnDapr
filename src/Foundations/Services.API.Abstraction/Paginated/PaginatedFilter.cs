using System.Text.Json.Serialization;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Paginated;

public class PaginatedFilter(int pageIndex = 0, int pageSize = 10)
{
    [JsonPropertyName("page_index")]
    public int PageIndex { get; set; } = pageIndex;
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; } = pageSize;
}
