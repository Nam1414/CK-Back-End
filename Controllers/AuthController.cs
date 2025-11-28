using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Entity;
using OrderManagementAPI.Models;
using OrderManagementAPI.Services;
using OrderManagementAPI.Helpers;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthController(AppDbContext context, JwtService jwtService, IEmailService emailService)
        {
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var err = ValidationHelper.ValidateLogin(request);
            if (err != null) return BadRequest(err);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null) return Unauthorized("Tài khoản không tồn tại");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Sai mật khẩu");

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
            var err = ValidationHelper.ValidateRegister(req);
            if (err != null) return BadRequest(err);

            if (await _context.Users.AnyAsync(x => x.Username == req.Username))
                return BadRequest("Username đã tồn tại");

            if (await _context.Users.AnyAsync(x => x.Email == req.Email))
                return BadRequest("Email đã được sử dụng");

            var user = new User
            {
                Username = req.Username,
                FullName = req.FullName,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công" });
        }

        // FORGOT PASSWORD → GỬI OTP EMAIL
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            var err = ValidationHelper.ValidateForgot(req);
            if (err != null) return BadRequest(err);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == req.Email);
            if (user == null) return BadRequest("Email không tồn tại");

            var otp = Random.Shared.Next(100000, 999999).ToString();
            user.ResetToken = otp;
            user.ResetTokenExpire = DateTime.UtcNow.AddMinutes(5);

            await _context.SaveChangesAsync();

            var subject = "Mã OTP đặt lại mật khẩu";
            var body = $@"
                <h3>Xin chào {user.FullName}</h3>
                <p>Mã OTP của bạn là:</p>
                <h2 style='color:blue'>{otp}</h2>
                <p>OTP có hiệu lực trong 5 phút.</p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return Ok(new { message = "Đã gửi OTP về email" });
        }

        // RESET PASSWORD (OTP)
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
        {
            var err = ValidationHelper.ValidateReset(req);
            if (err != null) return BadRequest(err);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == req.Email);
            if (user == null) return BadRequest("Email không tồn tại");

            if (user.ResetToken != req.Otp || user.ResetTokenExpire < DateTime.UtcNow)
                return BadRequest("OTP sai hoặc đã hết hạn");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpire = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }
    }
}
