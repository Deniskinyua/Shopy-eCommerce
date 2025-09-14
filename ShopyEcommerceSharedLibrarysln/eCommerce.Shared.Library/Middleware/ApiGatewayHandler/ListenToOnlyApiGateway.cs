using Microsoft.AspNetCore.Http;

namespace eCommerce.Shared.Library.Middleware.ApiGatewayHandler;

public class ListenToOnlyApiGateway (RequestDelegate next)
{
    //
    public async Task InvokeAsync(HttpContext httpContext)
    {
        // Extract header from the request
        var signedHeader = httpContext.Request.Headers["Api-Gateway"];
        // NULL : The request is not coming from the API Gateway
        if (signedHeader.FirstOrDefault() is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await httpContext.Response.WriteAsync("Sorry, service is unavailable");
            return;
        }
        else
        {
            await next(httpContext);
        }
    }
}