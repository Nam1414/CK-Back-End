using System.Security.Claims;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;

    public RoleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
            context.Items["UserRole"] = role;
        }

        await _next(context);
    }
}
