using System.Net;
using System.Text.Json;
using eCommerce.Shared.Library.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Shared.Library.Middleware.GlobalExceptionHandler;

public class GlobalException(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        //Declare Variables
        string message = "sorry, internal server error occured. Kindly try again";
        int statusCode = (int)HttpStatusCode.InternalServerError;
        string title = "Error";

        try
        {
            await next(httpContext);
            //Check if Exception is too many requests // status code 429
            if (httpContext.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            {
                title = "Warning";
                message = "Too many requests made";
                statusCode = (int)StatusCodes.Status429TooManyRequests;
                await ModifyHeader(httpContext, title, message, statusCode);
            }
            //Check if response is unauthorized => 401 
            if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                title = "alert";
                message = "You are not authorized to access this resource";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeader(httpContext, title, message, statusCode);
            }
            //Check if response is forbidden => 403
            if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                title = "Out of access";
                message = " You are not allowed to access this resource";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeader(httpContext, title, message, statusCode);
            }
        }
        catch (Exception exception)
        {
            //Log to file, debugger, console
          LogException.LogExceptions(exception);
          //Check if exception is timeout: => 408 request timeout
          if (exception is TaskCanceledException || exception is TimeoutException)
          {
              title = "Out of time";
              message = "Request timeout... try again";
              statusCode = StatusCodes.Status408RequestTimeout;
          }
          //Any other exception is handled by the default handler
          await ModifyHeader(httpContext, title, message, statusCode);
        }
    }

    private static async Task ModifyHeader(HttpContext httpContext, string title, string message, int statusCode)
    {
        //Display scary-free message to client
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
        {
            Detail = message, 
            Status = statusCode,
            Title = title
        }), CancellationToken.None);
        return;
    }
}