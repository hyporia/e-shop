namespace UserService.Api.Middleware;

public class NotFoundMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == StatusCodes.Status404NotFound)
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Not found");
        }
    }
}