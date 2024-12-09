using CurrencyApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/api/currencies",  () =>
{
    var currencies = new List<Currency>
    {
        new Currency() { CurrencyCode = "USD", CurrencyName = "USD Dollar", Rate = 1 },
        new Currency() { CurrencyCode = "NIS", CurrencyName = "New israeli shekel", Rate = 4.1 },
        new Currency() { CurrencyCode = "EUR", CurrencyName = "Euro", Rate = 1.1 }
    };
    return TypedResults.Ok(currencies);
});

app.Run();

