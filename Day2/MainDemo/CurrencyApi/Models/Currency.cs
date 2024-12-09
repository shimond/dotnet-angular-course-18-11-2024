namespace CurrencyApi.Models;

public record Currency
{
    public required string CurrencyCode { get; init; }
    public required string CurrencyName { get; init; }
    public double Rate{ get; init; }
}



