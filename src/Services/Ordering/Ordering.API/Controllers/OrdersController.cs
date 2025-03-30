using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[Authorize]
[ApiController]
public class OrdersController(
    ILogger<OrdersController> logger,
    IOrdersService ordersService
    ) : ControllerBase
{

    [Route("{orderNumber:int}/cancel")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelOrderAsync(int orderNumber)
    {
        bool result = await ordersService.CancelOrderAsync(orderNumber);
        if (result) return Ok();

        return BadRequest();
    }

    [Route("{orderNumber:int}/ship")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ShipOrderAsync(int orderNumber, [FromHeader(Name = "x-requestid")] string requestId)
    {
        bool result = await ordersService.ShipOrderAsync(orderNumber, requestId);
        if (result) return Ok();

        return BadRequest();
    }

    [Route("{orderNumber:int}")]
    [HttpGet]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetOrderAsync(int orderNumber)
    {
        var (isok, order) = await ordersService.GetOrderAsync(orderNumber);
        if (isok) return Ok(order);

        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IAsyncEnumerable<OrderSummary>), StatusCodes.Status200OK)]
    public async IAsyncEnumerable<OrderSummary> GetOrdersAsync()
    {
        var orderSummaries = ordersService.GetOrdersAsync();
        await foreach (var item in orderSummaries)
        {
            yield return item!;
        } 
    }

}
