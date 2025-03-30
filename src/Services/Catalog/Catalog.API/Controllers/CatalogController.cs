using Microsoft.eShopOnDapr.Services.API.Abstraction.Paginated;
using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CatalogController(ILogger<CatalogController> logger, ICatalogService catalogService) : ControllerBase
{
    [HttpGet("brands")]
    [ProducesResponseType(typeof(IAsyncEnumerable<CatalogBrandResp>), StatusCodes.Status200OK)]
    public IAsyncEnumerable<CatalogBrandResp> CatalogBrandsAsync() =>
        catalogService.GetCatalogBrandsAsync();

    [HttpGet("types")]
    [ProducesResponseType(typeof(IAsyncEnumerable<CatalogTypeResp>), StatusCodes.Status200OK)]
    public IAsyncEnumerable<CatalogTypeResp> CatalogTypesAsync() =>
        catalogService.GetCatalogTypesAsync();

    [HttpGet("items/by_ids")]
    [ProducesResponseType(typeof(IAsyncEnumerable<CatalogItemResp>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<IAsyncEnumerable<CatalogItem>> CatalogItemsAsync([FromQuery] string ids)
    {
        if (string.IsNullOrWhiteSpace(ids))
            return BadRequest("Ids value is invalid. Must be comma-separated list of numbers.");

        var items = catalogService.GetCatalogItemsAsync(ids);
        if(items != null)
            return Ok(items);

        return NoContent();
    }        

    [HttpGet("items/by_page")]
    [ProducesResponseType(typeof(PaginatedItems<CatalogItemResp>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<PaginatedItems<CatalogItemResp>?> ItemsAsync(
        [FromQuery] string typeId,
        [FromQuery] string brandId,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        PaginatedFilter filter = new()
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        var pageData = await catalogService.GetPaginatedCatalogItemsAsync(filter, typeId, brandId);
        return pageData;
    }
}
