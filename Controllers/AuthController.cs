using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Entity;
using OrderManagementAPI.Services; // Để dùng EmailService

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService; // Khai báo Service
        private readonly IConfiguration _config;

        // Inject Service vào Controller
        public AuthController(AppDbContext context, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });

            // Lấy Key từ appsettings.json
            var keyStr = _config["JwtSettings:Key"] ?? "Chuoi_Bi_Mat_Dai_It_Nhat_32_Ky_Tu_ABCXYZ_123456";
            var key = Encoding.ASCII.GetBytes(keyStr);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token), user = new { id = user.Id, username = user.Username, role = user.Role, fullName = user.FullName } });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại" });
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { message = "Email đã được sử dụng" });

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đăng ký thành công" });
        }

        // --- GỬI EMAIL THẬT ---
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return BadRequest(new { message = "Email không tồn tại" });

            var otp = Random.Shared.Next(100000, 999999).ToString();
            user.ResetToken = otp;
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(5);
            await _context.SaveChangesAsync();

            // Gửi Email
            try 
            {
                string subject = "Mã xác nhận Quên mật khẩu";
                string body = $@"
                    <h3>Xin chào {user.FullName},</h3>
                    <p>Bạn vừa yêu cầu đặt lại mật khẩu.</p>
                    <p>Mã OTP của bạn là: <b style='font-size: 20px; color: blue;'>{otp}</b></p>
                    <p>Mã này có hiệu lực trong 5 phút.</p>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Lỗi gửi mail: " + ex.Message });
            }

            return Ok(new { message = "Đã gửi mã OTP vào Email của bạn." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return BadRequest(new { message = "User không tồn tại" });

            if (user.ResetToken != dto.Otp || user.ResetTokenExpiry < DateTime.Now)
                return BadRequest(new { message = "Mã OTP sai hoặc hết hạn" });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }
    }
}