public class NotFoundMiddleware
{
    private readonly RequestDelegate _next;

    public NotFoundMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 404)
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Not found");
        }
    }
}