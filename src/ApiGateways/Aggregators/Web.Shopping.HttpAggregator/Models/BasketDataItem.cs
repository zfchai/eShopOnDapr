namespace Microsoft.eShopOnDapr.Web.Shopping.HttpAggregator.Models;

public record BasketDataItem(string ProductId, string ProductName, decimal UnitPrice, int Quantity, string PictureFileName);
