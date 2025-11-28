using System.Security.Claims;

namespace ProductAPI.Middlewares
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

            // Ví dụ: chặn DELETE nếu không phải Admin
            if (context.Request.Method == "DELETE" && role != "Admin")
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Chỉ Admin mới được phép xóa");
                return;
            }

            await _next(context);
        }
    }
}
