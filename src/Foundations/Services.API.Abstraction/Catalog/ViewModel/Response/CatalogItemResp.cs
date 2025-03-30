namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Catalog.ViewModel.Response;

public record CatalogItemResp(
    string Id,
    string Name,
    decimal Price,
    string PictureFileName);
