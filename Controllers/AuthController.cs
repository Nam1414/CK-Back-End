using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Tìm user trong DataStore
            var user = DataStore.Users.FirstOrDefault(u => 
                u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng" });
            }

            // Tạo JWT token
            var token = _jwtService.GenerateToken(user.Username, user.Role);

            return Ok(new LoginResponse
            {
                Token = token,
                Role = user.Role,
                Username = user.Username
            });
        }
    }
}