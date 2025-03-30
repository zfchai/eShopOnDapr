using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Mappers;

internal static class CatalogTypeMapper
{
    public static CatalogTypeResp To(this CatalogType req)
    {
        return new CatalogTypeResp(req.Id, req.Name);
    }
}
