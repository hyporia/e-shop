namespace UserService.Api.Middleware;

public class NotFoundMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == 404)
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Not found");
        }
    }
}