using CatalogApi.Models;

namespace CatalogApi.Contracts;

public interface IProductsRepository
{
    Task<List<Product>> GetAllProducts();
    Task<Product?> GetProductById(int id);
    Task<Product> CreateNewProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task<Product> DeleteProduct(int id);
    Task<List<Product>> SearchProduct(string name);
    
}
