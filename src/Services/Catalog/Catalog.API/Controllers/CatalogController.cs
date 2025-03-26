namespace Microsoft.eShopOnDapr.Services.Catalog.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CatalogController(ILogger<CatalogController> logger, ICatalogService catalogService) : ControllerBase
{
    [HttpGet("brands")]
    [ProducesResponseType(typeof(List<CatalogBrand>), StatusCodes.Status200OK)]
    public Task<List<CatalogBrand>> CatalogBrandsAsync() =>
        catalogService.GetCatalogBrandsAsync();

    [HttpGet("types")]
    [ProducesResponseType(typeof(List<CatalogType>), StatusCodes.Status200OK)]
    public Task<List<CatalogType>> CatalogTypesAsync() =>
        catalogService.GetCatalogTypesAsync();

    [HttpGet("items/by_ids")]
    [ProducesResponseType(typeof(List<CatalogItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<List<CatalogItem>>> ItemsAsync([FromQuery] string ids)
    {
        if (string.IsNullOrWhiteSpace(ids))
            return BadRequest("Ids value is invalid. Must be comma-separated list of numbers.");

        var items = await catalogService.GetCatalogItemsAsync(ids);
        if(items != null || items?.Count > 0)
            return Ok(items);

        return NoContent();
    }        

    [HttpGet("items/by_page")]
    [ProducesResponseType(typeof(PaginatedItemsViewModel), StatusCodes.Status200OK)]
    public async Task<PaginatedItemsViewModel> ItemsAsync(
        [FromQuery] int typeId = -1,
        [FromQuery] int brandId = -1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var pageData = await catalogService.GetPaginatedCatalogItemsAsync(typeId, brandId, pageSize, pageIndex);
        
        return pageData;
    }
}
