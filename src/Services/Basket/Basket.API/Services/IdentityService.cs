namespace Microsoft.eShopOnDapr.Services.Basket.API.Services;

public class IdentityService(ILogger<IdentityService> logger, IServiceProvider sp) : IIdentityService
{
    private readonly IHttpContextAccessor _context = sp.GetRequiredService<IHttpContextAccessor>();
    private readonly IBasketRepository _repository = sp.GetRequiredService<IBasketRepository>();
    private readonly IEventBus _eventBus = sp.GetRequiredService<IEventBus>();

    public string GetUserIdentity()
    {
        return _context.HttpContext?.User.FindFirst("sub")?.Value ?? string.Empty;
    }

    public async Task<CustomerBasketResp> GetBasketAsync() 
    {
        var userId = GetUserIdentity();
        var basket = await _repository.GetBasketAsync(userId);
        return basket ?? new CustomerBasketResp(userId);
    }

    public async Task<CustomerBasketResp> UpdateBasketAsync(CustomerBasketReq value)
    {
        var userId = GetUserIdentity();
        value.BuyerId = userId;
        return await _repository.UpdateBasketAsync(value);
    }

    public async Task<int> CheckoutAsync(BasketCheckoutReq basketCheckout, string requestId) 
    {
        var userId = GetUserIdentity();
        var basket = await _repository.GetBasketAsync(userId);
        if (basket == null)
        {
            return StatusCodes.Status400BadRequest;
        }

        var eventRequestId = Guid.TryParse(requestId, out Guid parsedRequestId)
            ? parsedRequestId : Guid.NewGuid();

        var eventMessage = new UserCheckoutAcceptedIntegrationEvent(
            userId,
            basketCheckout.UserEmail,
            basketCheckout.City,
            basketCheckout.Street,
            basketCheckout.State,
            basketCheckout.Country,
            basketCheckout.CardNumber,
            basketCheckout.CardHolderName,
            basketCheckout.CardExpiration,
            basketCheckout.CardSecurityCode,
            eventRequestId,
            basket);

        // Once basket is checkout, sends an integration event to
        // ordering.api to convert basket to order and proceed with
        // order creation process
        await _eventBus.PublishAsync(eventMessage);

        return StatusCodes.Status202Accepted;
    }

    public async Task DeleteBasketAsync()
    {
        var userId = GetUserIdentity();
        logger.LogInformation("Deleting basket for user {UserId}...", userId);

        await _repository.DeleteBasketAsync(userId);
    }

}
