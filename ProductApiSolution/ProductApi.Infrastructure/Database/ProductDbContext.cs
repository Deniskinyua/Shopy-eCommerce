using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Database;

public class ProductDbContext  : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }
    
}


