namespace Microsoft.eShopOnDapr.Services.Basket.API.Controllers;

[Route("api/v1/[controller]")]
[Authorize(Policy = "ApiScope")]
[ApiController]
public class BasketController(ILogger<BasketController> logger, IIdentityService identityService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
    public async Task<ActionResult<CustomerBasket>> GetBasketAsync()
    {
        var basket = await identityService.GetBasketAsync();
        return Ok(basket);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
    public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
    {
        var basket = await identityService.UpdateBasketAsync(value);
        return Ok(basket);
    }

    [HttpPost("checkout")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CheckoutAsync(
        [FromBody] BasketCheckout basketCheckout,
        [FromHeader(Name = "X-Request-Id")] string requestId)
    {
        int statusCode = await identityService.CheckoutAsync(basketCheckout, requestId);

        return statusCode switch
        {
            StatusCodes.Status202Accepted => Accepted(),
            StatusCodes.Status400BadRequest => BadRequest(),
            _ => NoContent()
        };
    }

    // DELETE api/values/5
    [HttpDelete]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task DeleteBasketAsync()
    {
        await identityService.DeleteBasketAsync();
    }
}
