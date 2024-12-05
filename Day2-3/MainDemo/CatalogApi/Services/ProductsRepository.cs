using CatalogApi.Contracts;
using CatalogApi.DataContext;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Services;

public class ProductsRepository (CatalogDbContext dbContext): IProductsRepository
{
    public async Task<Product> CreateNewProduct(Product product)
    {
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public Task<Product> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Product>> GetAllProducts()
    {
        var products = await dbContext.Products.ToListAsync();
        return products;
    }

    public async Task<Product?> GetProductById(int id)
    {
        var p = await dbContext.Products.FirstOrDefaultAsync(x=> x.Id == id);
        return p;
        
    }

    public Task<List<Product>> SearchProduct(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Product> UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }
}
