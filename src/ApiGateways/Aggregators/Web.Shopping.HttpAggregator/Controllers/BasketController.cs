namespace Microsoft.eShopOnDapr.Web.Shopping.HttpAggregator.Controllers;

[Route("api/v1/[controller]")]
[Authorize]
[ApiController]
public sealed class BasketController(ILogger<BasketController> logger, IServiceProvider sp) : ControllerBase
{
    private readonly ICatalogService _catalog = sp.GetRequiredService<ICatalogService>();

    [HttpPost, HttpPut]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BasketData), StatusCodes.Status200OK)]
    public async Task<ActionResult<BasketData>> UpdateAllBasketAsync(
        [FromHeader] string authorization,
        [FromBody] UpdateBasketRequest data)
    {
        var (statusCode, msg, basket) = await _catalog.UpdateAllBasketAsync(data, authorization);

        if (statusCode == StatusCodes.Status400BadRequest)
        {
            logger.LogError($"StatusCode={statusCode}, error:{msg}");
            return BadRequest(msg);
        }

        return Ok(basket);
    }
}
