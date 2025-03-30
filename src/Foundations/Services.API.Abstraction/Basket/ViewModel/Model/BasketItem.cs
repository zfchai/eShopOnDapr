using System.ComponentModel.DataAnnotations;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Basket.ViewModel.Model;

public class BasketItem : IValidatableObject
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string PictureFileName { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (Quantity < 1)
        {
            results.Add(new ValidationResult("Invalid number of units", ["Quantity"]));
        }

        return results;
    }
}
