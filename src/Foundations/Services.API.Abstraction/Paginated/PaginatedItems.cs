using System.Text.Json.Serialization;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Paginated;

public class PaginatedItems<TItem>(PaginatedFilter filter, long count, IEnumerable<TItem>? items) where TItem : class
{
    [JsonPropertyName("page_index")]
    public int PageIndex { get; set; } = filter.PageIndex;
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; } = filter.PageSize;
    [JsonPropertyName("count")]
    public long Count { get; set; } = count;
    [JsonPropertyName("items")]
    public IEnumerable<TItem>? Items { get; set; } = items;

}
