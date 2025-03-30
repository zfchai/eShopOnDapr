using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Mappers;

public static class CatalogTypeMapper
{
    public static CatalogTypeResp To(this CatalogType req)
    {
        return new CatalogTypeResp
        {
            Id = req.Id,
            Name = req.Name
        };
    }
}
