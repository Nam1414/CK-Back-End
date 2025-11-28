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
                Role = "User",
                PhoneNumber = dto.PhoneNumber
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đăng ký thành công" });
        }

        // --- GỬI EMAIL THẬT ---
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            // Tìm User trong DB (So khớp cả cột Email và cột PhoneNumber)
            var user = await _context.Users.FirstOrDefaultAsync(u => 
                u.Email == dto.Identity || u.PhoneNumber == dto.Identity);

            if (user == null) return BadRequest(new { message = "Thông tin không tồn tại trong hệ thống" });

            // Tạo OTP
            var otp = Random.Shared.Next(100000, 999999).ToString();
            user.ResetToken = otp;
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(5);
            await _context.SaveChangesAsync();

            // Kiểm tra xem người dùng nhập Email hay SĐT (dựa vào ký tự @)
            bool isEmail = dto.Identity.Contains("@");

            if (isEmail)
            {
                // === TRƯỜNG HỢP EMAIL: Gửi mail thật ===
                try 
                {
                    string subject = "Mã xác nhận Quên mật khẩu";
                    string body = $@"
                        <h3>Xin chào {user.FullName},</h3>
                        <p>Mã OTP của bạn là: <b style='color:red; font-size:20px;'>{otp}</b></p>
                        <p>Mã này hết hạn sau 5 phút.</p>";
                    
                    // Gọi Service gửi mail (Đã cấu hình Gmail App Password)
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                    
                    return Ok(new { message = "Mã OTP đã được gửi vào Email của bạn." });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Lỗi gửi mail: " + ex.Message });
                }
            }
            else
            {
                // === TRƯỜNG HỢP SỐ ĐIỆN THOẠI: In ra Terminal (Giả lập SMS) ===
                Console.WriteLine("========================================");
                Console.WriteLine($"[SMS GATEWAY] Đang gửi tin nhắn tới số: {user.PhoneNumber}");
                Console.WriteLine($"[NỘI DUNG SMS] Mã xác nhận của bạn là: {otp}");
                Console.WriteLine("========================================");

                return Ok(new { message = "Mã OTP đã được gửi qua SMS (Vui lòng xem Terminal để lấy mã)" });
            }
        }

        // 4. ĐẶT LẠI MẬT KHẨU (Sửa lại để tìm theo Identity)
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            // Tìm user theo Email hoặc Phone
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Identity || u.PhoneNumber == dto.Identity);
            
            if (user == null) return BadRequest(new { message = "Tài khoản không tồn tại" });

            if (user.ResetToken != dto.Otp || user.ResetTokenExpiry < DateTime.Now)
            {
                return BadRequest(new { message = "Mã OTP sai hoặc đã hết hạn" });
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }
    }
}