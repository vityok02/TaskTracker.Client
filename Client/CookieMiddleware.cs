namespace Client;

public class CookieMiddleware
{
    private readonly RequestDelegate _next;
    private const string Key = "jwtToken";

    public CookieMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/set-token")
        {
            var token = context.Request.Query["token"]
                .ToString();

            if (!string.IsNullOrEmpty(token))
            {
                context.Response.Cookies
                    .Append(Key, token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });
            }

            context.Response.Redirect("/");
            return;
        }

        if (context.Request.Path == "/logout")
        {
            context.Response.Cookies.Delete(Key);
            context.Response.Redirect("/");
            return;
        }

        await _next(context);
    }
}
