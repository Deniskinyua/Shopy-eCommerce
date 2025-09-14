using eCommerce.Shared.Library.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Database;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        // Add database connectivity
        // Add Authentication Scheme
        SharedServiceContainer.AddSharedServices<ProductDbContext>(serviceCollection, configuration, configuration["MySerilog:FileName"]!);
        //
        serviceCollection.AddScoped<IProduct, ProductRepository>();
        return serviceCollection;
    }

    public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder applicationBuilder)
    {
        //Register middleware => Global Exception handler
        // Listen to API Gateway
        SharedServiceContainer.UseSharedPolicies(applicationBuilder);
        return applicationBuilder;
    }
}