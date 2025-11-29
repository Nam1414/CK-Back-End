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
        // Các dependency cần thiết cho authentication
        private readonly AppDbContext _context;           // DbContext để truy cập database Users
        private readonly IEmailService _emailService;      // Service gửi email OTP (Gmail/SMTP)
        private readonly IConfiguration _config;           // Đọc config từ appsettings.json (JWT Key)

        // Constructor Dependency Injection - Inject tất cả services cần thiết
        public AuthController(AppDbContext context, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        /// API ĐĂNG NHẬP - Tạo JWT Token 7 ngày
        /// Endpoint: POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // BƯỚC 1: Tìm user theo Username và verify password bằng BCrypt
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });

            // BƯỚC 2: Tạo JWT Token với Claims (Id, Username, Role)
            var keyStr = _config["JwtSettings:Key"] ?? "Chuoi_Bi_Mat_Dai_It_Nhat_32_Ky_Tu_ABCXYZ_123456";
            var key = Encoding.ASCII.GetBytes(keyStr);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // User ID
                    new Claim(ClaimTypes.Name, user.Username),                 // Username
                    new Claim(ClaimTypes.Role, user.Role)                      // Role (Admin/User)
                }),
                Expires = DateTime.UtcNow.AddDays(7),  // Token hết hạn sau 7 ngày
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // TRẢ VỀ: Token + thông tin user (không trả password)
            return Ok(new { 
                token = tokenHandler.WriteToken(token), 
                user = new { id = user.Id, username = user.Username, role = user.Role, fullName = user.FullName } 
            });
        }

        /// API ĐĂNG KÝ TÀI KHOẢN MỚI
        /// Endpoint: POST /api/auth/register
        /// Kiểm tra trùng Username/Email trước khi tạo
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // BƯỚC 1: VALIDATE - Check trùng Username hoặc Email
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại" });
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { message = "Email đã được sử dụng" });

            // BƯỚC 2: TẠO USER MỚI với Password đã hash bằng BCrypt
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),  // Hash password trước khi lưu
                FullName = dto.FullName,
                Role = "User",           // Default role là User (không phải Admin)
                PhoneNumber = dto.PhoneNumber
            };

            // BƯỚC 3: Lưu vào DB
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Đăng ký thành công" });
        }

        /// API QUÊN MẬT KHẨU - GỬI OTP QUA EMAIL/SMS
        /// Endpoint: POST /api/auth/forgot-password
        /// Hỗ trợ cả Email và Số điện thoạiy>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            // BƯỚC 1: Tìm user theo Email HOẶC PhoneNumber (Identity linh hoạt)
            var user = await _context.Users.FirstOrDefaultAsync(u => 
                u.Email == dto.Identity || u.PhoneNumber == dto.Identity);

            if (user == null) 
                return BadRequest(new { message = "Thông tin không tồn tại trong hệ thống" });

            // BƯỚC 2: TẠO OTP 6 chữ số + Lưu vào DB với thời hạn 5 phút
            var otp = Random.Shared.Next(100000, 999999).ToString();
            user.ResetToken = otp;
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(5);  // Hết hạn sau 5 phút
            await _context.SaveChangesAsync();

            // BƯỚC 3: PHÂN BIỆT EMAIL hay SMS (dựa vào ký tự @)
            bool isEmail = dto.Identity.Contains("@");

            if (isEmail)
            {
                // === EMAIL: Gửi mail thật qua Gmail SMTP ===
                try 
                {
                    string subject = "Mã xác nhận Quên mật khẩu";
                    string body = $@"
                        <h3>Xin chào {user.FullName},</h3>
                        <p>Mã OTP của bạn là: <b style='color:red; font-size:20px;'>{otp}</b></p>
                        <p>Mã này hết hạn sau 5 phút.</p>";
                    
                    // Gọi EmailService (đã config Gmail App Password)
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
                // === SMS: Giả lập - In ra Terminal (thay bằng SMS Gateway thật sau)
                Console.WriteLine("========================================");
                Console.WriteLine($"[SMS GATEWAY] Đang gửi tin nhắn tới số: {user.PhoneNumber}");
                Console.WriteLine($"[NỘI DUNG SMS] Mã xác nhận của bạn là: {otp}");
                Console.WriteLine("========================================");

                return Ok(new { message = "Mã OTP đã được gửi qua SMS (Vui lòng xem Terminal để lấy mã)" });
            }
        }

        /// API ĐẶT LẠI MẬT KHẨU bằng OTP
        /// Endpoint: POST /api/auth/reset-password
        /// Verify OTP + Cập nhật password mới
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            // BƯỚC 1: Tìm user theo Email hoặc Phone
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Identity || u.PhoneNumber == dto.Identity);
            
            if (user == null) 
                return BadRequest(new { message = "Tài khoản không tồn tại" });

            // BƯỚC 2: VALIDATE OTP - Phải đúng và chưa hết hạn
            if (user.ResetToken != dto.Otp || user.ResetTokenExpiry < DateTime.Now)
            {
                return BadRequest(new { message = "Mã OTP sai hoặc đã hết hạn" });
            }

            // BƯỚC 3: CẬP NHẬT PASSWORD MỚI + XÓA OTP
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ResetToken = null;           // Xóa OTP sau khi dùng
            user.ResetTokenExpiry = null;     // Xóa thời hạn
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }
    }
}
