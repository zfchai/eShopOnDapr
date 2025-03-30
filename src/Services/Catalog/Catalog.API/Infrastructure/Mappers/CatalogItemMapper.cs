using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Mappers;

public static class CatalogItemMapper
{
   
    public static CatalogItemResp To(this CatalogItem entity)
    {
        return new CatalogItemResp(
            entity.Id,
            entity.Name,
            entity.Price,
            entity.PictureFileName);
    }
}
