using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Models;
using Microsoft.eShopOnDapr.Services.Ordering.API.ViewModel.Response;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Mappers;

public static class OrderSummaryMapper
{
    public static OrderSummaryResp To(this OrderSummary orderSummary) => new 
    (
       orderSummary.OrderNumber,
       orderSummary.OrderDate,
       orderSummary.OrderStatus,
       orderSummary.Total
    );
}
