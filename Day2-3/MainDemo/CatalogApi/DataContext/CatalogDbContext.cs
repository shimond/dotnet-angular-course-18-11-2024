using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.DataContext;

public class CatalogDbContext : DbContext
{

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }

}
