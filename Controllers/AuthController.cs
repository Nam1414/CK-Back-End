using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Entity;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // 1. ĐĂNG NHẬP
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // Tìm user theo username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

            // Kiểm tra user tồn tại VÀ mật khẩu khớp (dùng BCrypt)
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu" });
            }

            // Tạo Token
            var tokenHandler = new JwtSecurityTokenHandler();
            // KHÓA BÍ MẬT: Phải khớp với Program.cs (nên dài > 32 ký tự)
            var key = Encoding.ASCII.GetBytes("Chuoi_Bi_Mat_Dai_It_Nhat_32_Ky_Tu_ABCXYZ_123456"); 
            
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
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Trả về JSON (viết thường chữ cái đầu cho chuẩn JS)
            return Ok(new 
            { 
                token = tokenHandler.WriteToken(token), 
                user = new 
                { 
                    id = user.Id,
                    username = user.Username, 
                    fullName = user.FullName, 
                    role = user.Role 
                } 
            });
        }

        // 2. ĐĂNG KÝ
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // Kiểm tra trùng tên
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại" });
            }

            // Tạo User mới với mật khẩu đã mã hóa
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // Mã hóa pass
                FullName = dto.FullName,
                Role = "User" // Mặc định là User thường
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công" });
        }
    }
}