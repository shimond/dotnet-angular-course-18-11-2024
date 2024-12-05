using CatalogApi.Contracts;
using CatalogApi.DataContext;
using CatalogApi.Routes;
using CatalogApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<CatalogDbContext>(o => o.UseInMemoryDatabase("catalog"));
builder.Services.AddDbContext<CatalogDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDb")));

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Services.CreateScope().ServiceProvider.GetRequiredService<CatalogDbContext>().Database.EnsureCreated();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapProducts();


app.Run();

