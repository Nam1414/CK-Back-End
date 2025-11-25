using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        //LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Unauthorized(new { message = "Tài khoản không tồn tại" });

            // verify password hash
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Sai mật khẩu" });

            var token = _jwtService.GenerateToken(user.Username, user.Role);

            return Ok(new LoginResponse
            {
                Username = user.Username,
                Role = user.Role,
                Token = token
            });
        }

        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (await _context.Users.AnyAsync(x => x.Username == req.Username))
                return BadRequest("Username đã tồn tại");

            if (await _context.Users.AnyAsync(x => x.Email == req.Email))
                return BadRequest("Email đã tồn tại");

            var user = new User
            {
                Username = req.Username,
                Email = req.Email,
                Role = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Đăng ký thành công" });
        }

        // FORGOT PASSWORD
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == req.Email);

            if (user == null)
                return NotFound("Email không tồn tại");

            var token = Guid.NewGuid().ToString();

            user.ResetToken = token;
            user.ResetTokenExpire = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đã tạo mã reset. Dùng mã này gọi API reset-password.",
                token = token
            });
        }
        // RESET PASSWORD
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ResetToken == req.Token);

            if (user == null)
                return BadRequest("Token không hợp lệ");

            if (!user.ResetTokenExpire.HasValue || user.ResetTokenExpire < DateTime.UtcNow)
                return BadRequest("Token đã hết hạn");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpire = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt lại mật khẩu thành công" });
        }
    }
}
