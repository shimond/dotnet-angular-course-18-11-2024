var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServiceDiscovery();

builder.Services.AddHttpClient("catalogApi", x => {
    x.BaseAddress = new Uri("https://catalogApi");
}).AddServiceDiscovery();

builder.Services.AddHttpClient("currencyApi", x => {
    x.BaseAddress = new Uri("https://currencyApi");
}).AddServiceDiscovery();


var app = builder.Build();

app.MapDefaultEndpoints();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();


app.MapGet("/api/products/{currnecyCode}", async (string currnecyCode, IHttpClientFactory factory, IConfiguration configuration) => {
    var productsClient = factory.CreateClient("catalogApi");
    var currenciesClient = factory.CreateClient("currencyApi");
    var resOfProducts = await productsClient.GetFromJsonAsync<List<ProductDto>>("/api/products");
    var resOfCurrencies = await currenciesClient.GetFromJsonAsync<List<CurrencyDto>>("/api/currencies");
    if(!resOfCurrencies.Any(x=>x.CurrencyCode == currnecyCode))
    {
        return Results.NotFound();
    }
    var selectedCurrency = resOfCurrencies.First(x => x.CurrencyCode == currnecyCode);
    var result = resOfProducts.Select(x => {

        return x with { Price = x.Price * selectedCurrency.Rate };
    }).ToList();

    return Results.Ok(result);
});


app.Run();


public record ProductDto(int Id,string Name ,string Description ,double Price );
public record CurrencyDto(string CurrencyCode,string CurrencyName ,double Rate );


