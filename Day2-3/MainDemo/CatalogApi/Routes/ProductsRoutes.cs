using CatalogApi.Contracts;
using CatalogApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CatalogApi.Routes;

public static class ProductsRoutes
{
    public static IEndpointRouteBuilder MapProducts(this IEndpointRouteBuilder endpoints)
    {
        var productGroup = endpoints.MapGroup("api/products");
        productGroup.MapGet("", GetAll);
        productGroup.MapPost("", AddNewProduct);
        productGroup.MapGet("{id}", GetById);
        productGroup.MapGet("search/{term}", Search);
        return endpoints;
    }
    static async Task<Ok<List<Product>>> Search(IProductsRepository rep, string term)
    {
        var res = await rep.SearchProduct(term);
        return TypedResults.Ok(res);
    }
    static async Task<Ok<List<Product>>> GetAll(IProductsRepository rep)
    {
        var res = await rep.GetAllProducts();
        return TypedResults.Ok(res);
    }
    static async Task<Results<NotFound, Ok<Product>>> GetById(int id, IProductsRepository repository)
    {
        var p = await repository.GetProductById(id);
        if (p is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(p);
    }

    static async Task<Created<Product>> AddNewProduct(IProductsRepository rep, Product p)
    {
        var res = await rep.CreateNewProduct(p);
        return TypedResults.Created($"api/products/{res.Id}", res);
    }
}
