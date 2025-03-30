using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Mappers;

public static class CatalogBrandMapper
{
    public static CatalogBrand From(this CatalogBrandReq req)
    {
        return new CatalogBrand(
            req.Id ?? Guid.NewGuid().ToString(),
            req.Name
        );
    }

    public static CatalogBrandResp To(this CatalogBrand entity)
    {
        return new CatalogBrandResp() 
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }


}
