namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

public class CatalogItem(
    string id,
    string name,
    decimal price,
    string pictureFileName,
    string catalogTypeId,
    string catalogBrandId,
    int availableStock)
{
    public string Id { get; private set; } = id;

    public string Name { get; private set; } = name;

    public decimal Price { get; private set; } = price;

    public string PictureFileName { get; private set; } = pictureFileName;

    public string CatalogTypeId { get; private set; } = catalogTypeId;

    public CatalogType CatalogType { get; private set; } = null!;

    public string CatalogBrandId { get; private set; } = catalogBrandId;

    public CatalogBrand CatalogBrand { get; private set; } = null!;

    public int AvailableStock { get; private set; } = availableStock;

    /// <summary>
    /// Simply decrement the quantity of a particular item in inventory.
    /// We don't care if we run out of stock.
    /// </summary>
    public int RemoveStock(int quantityDesired)
    {
        AvailableStock -= quantityDesired;

        return quantityDesired;
    }
}
