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
        var path = context.Request.Path.Value?.ToLower();
        var method = context.Request.Method;

        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

        // Ví dụ: chặn DELETE bất kỳ nếu không phải Admin
        if (method == "DELETE" && role != "Admin")
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Chỉ Admin mới được phép xóa");
            return;
        }

        await _next(context);
    }
}
