namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Models;

public class Address(
    string street,
    string city,
    string state,
    string country)
{
    public string Street { get; private set; } = street;
    public string City { get; private set; } = city;
    public string State { get; private set; } = state;
    public string Country { get; private set; } = country;

    public Address() : this(
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty)
    {
    }
}
