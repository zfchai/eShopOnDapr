namespace Microsoft.eShopOnDapr.Services.Catalog.API.ViewModel.Response;

public record CatalogItemResp(
    string Id,
    string Name,
    decimal Price,
    string PictureFileName);
