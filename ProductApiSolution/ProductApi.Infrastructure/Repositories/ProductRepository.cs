using System.Linq.Expressions;
using eCommerce.Shared.Library.Logs;
using eCommerce.Shared.Library.Responses;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Database;

namespace ProductApi.Infrastructure.Repositories;

public class ProductRepository(ProductDbContext productDbContext) : IProduct
{
    /**
     * CreateAsync => Creating a new product
     */
    public async Task<Response> CreateAsync(Product entity)
    {
        try
        {
            var getProduct = await GetByAsync(product => product.Name!.Equals(entity.Name));
            //Check if the product already exists
            if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                return new Response(false, $"{entity.Name} already exists");
            var currentEntity = productDbContext.Products.Add(entity).Entity;
            await productDbContext.SaveChangesAsync();
            if (currentEntity is not null && currentEntity.Id > 0)
                return new Response(true, $"{entity.Name} added to the database successfully");
            else
                return new Response(false, $"Error adding {entity.Name}");
        }
        catch (Exception exception)
        {
            //Log exception
            LogException.LogExceptions(exception);
            return new Response(false, "Error occurred while adding new product");
        }
    }
    /**
     * UpdateAsync => Update product details;
     */
    public async Task<Response> UpdateAsync(Product entity)
    {
        try
        {
            var product = await FindByIdAsync(entity.Id);
            if (product is null) return new Response(false, $"{entity.Name} not found.");
            productDbContext.Entry(product).State = EntityState.Detached;
            productDbContext.Products.Update(entity);
            await productDbContext.SaveChangesAsync();
            return new Response(true, $"{entity.Name} is updated successfully");
        }
        catch (Exception exception)
        {
            LogException.LogExceptions(exception);
            return new Response(false, "Error occured while updating product");
        }
    }
    
    /**
     * DeleteAsync => Deleting a product
     */
    public async Task<Response> DeleteAsync(Product entity)
    {
        try
        {
            var product = await  FindByIdAsync(entity.Id);
            if (product is null) return new Response(false, $"{entity.Name} does not exist");
            productDbContext.Products.Remove(product);
            await productDbContext.SaveChangesAsync();
            return new Response(true, $"{entity.Name} deleted successfully");
        }
        catch (Exception exception)
        {
            LogException.LogExceptions(exception);
            return new Response(false, "Error occured while deleting the product");
        }
    }
    /**
     * GetAllAsync => Get all products.
     */
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            var products = await productDbContext.Products.AsNoTracking().ToListAsync();
            return products is not null ? products : null!;
        }
        catch (Exception exception)
        {
            LogException.LogExceptions(exception);
            throw new Exception("Error retrieving products");
        }
    }
    /**
     * FindByIdAsync => Find product by product id;
     */
    public async Task<Product> FindByIdAsync(int productId)
    {
        try
        {
            var product = await productDbContext.Products.FindAsync(productId);
            return product is not null ? product : null!;
        }
        catch (Exception exception)
        {
            LogException.LogExceptions(exception);
            throw new Exception("Error retrieving the product"); // !TODO Create a custom Exception
        }
    }
    /**
     * GetByAsync => Retrieve the product by any value
     */
    public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
    {
        try
        {
            var product = await productDbContext.Products.Where(predicate).FirstOrDefaultAsync()!;
            return product is not null ? product : null!;
        }
        catch (Exception exception)
        {
            LogException.LogExceptions(exception);
            throw new Exception("Error retrieving product");
        }
    }
}