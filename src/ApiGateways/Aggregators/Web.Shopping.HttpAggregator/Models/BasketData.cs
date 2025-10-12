namespace Microsoft.eShopOnDapr.Web.Shopping.HttpAggregator.Models;

/// <summary>
/// 购物车数据
/// </summary>
public class BasketData
{
    public List<BasketDataItem> Items { get; set; } = [];
}
