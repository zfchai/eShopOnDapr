namespace Microsoft.eShopOnDapr.Services.Catalog.API.Model;

public class CatalogItem(
    int id,
    string name,
    decimal price,
    string pictureFileName,
    int catalogTypeId,
    int catalogBrandId,
    int availableStock)
{
    public int Id { get; private set; } = id;

    public string Name { get; private set; } = name;

    public decimal Price { get; private set; } = price;

    public string PictureFileName { get; private set; } = pictureFileName;

    public int CatalogTypeId { get; private set; } = catalogTypeId;

    public CatalogType CatalogType { get; private set; } = null!;

    public int CatalogBrandId { get; private set; } = catalogBrandId;

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
