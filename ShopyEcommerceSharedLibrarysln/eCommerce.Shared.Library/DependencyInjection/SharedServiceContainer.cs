using eCommerce.Shared.Library.Middleware.ApiGatewayHandler;
using eCommerce.Shared.Library.Middleware.GlobalExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using Serilog;

namespace eCommerce.Shared.Library.DependencyInjection;

public static class SharedServiceContainer
{
    public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection serviceCollection,
        IConfiguration configuration, string fileName) where TContext : DbContext
    {
        // Add Genetic Database Context
        serviceCollection.AddDbContext<TContext>(
            option => option.UseMySql(
                configuration.GetConnectionString("eCommerceConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("eCommerceConnection")),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null // Add specific MySQL error codes 
                    )
                )
            );
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File(path: $"{fileName}-.text", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3] {message :lj} {NewLine}{Exception}",
                rollingInterval: RollingInterval.Day).CreateLogger();
        
        //Add JwtAuthenticationScheme
        JwtAuthenticationScheme.AddJwtAuthenticationScheme(serviceCollection, configuration);
        
        return serviceCollection;
    }

    public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder applicationBuilder)
    {
        //Use Global Exception
        applicationBuilder.UseMiddleware<GlobalException>();
        //Register middleware to block all outsider API calls
        applicationBuilder.UseMiddleware<ListenToOnlyApiGateway>();
        return applicationBuilder;
    }
}