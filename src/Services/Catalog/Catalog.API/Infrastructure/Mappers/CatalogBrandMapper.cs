using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Mappers;

internal static class CatalogBrandMapper
{
    public static CatalogBrand From(this CatalogBrandReq req)
    {
        string id = req.Id.IfNullOrWhiteSpaceAs(Guid.CreateVersion7().ToString());
        return new CatalogBrand(id, req.Name);
    }

    public static CatalogBrandResp To(this CatalogBrand entity)
    {
        return new CatalogBrandResp(entity.Id, entity.Name);
    }


}
