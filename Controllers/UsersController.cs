using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Entity;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bắt buộc đăng nhập
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LẤY THÔNG TIN CÁ NHÂN
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            return Ok(new 
            { 
                user.Id, user.Username, user.FullName, user.Email, user.Role 
            });
        }

        // 2. CẬP NHẬT THÔNG TIN (Tên & Email)
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // Nếu đổi Email, phải kiểm tra xem Email mới có bị trùng không
            if (user.Email != dto.Email && await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest(new { message = "Email này đã được sử dụng bởi tài khoản khác" });
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
            
            // Trả về user mới để Front-end cập nhật
            return Ok(new { message = "Cập nhật thông tin thành công", user = new { user.Id, user.Username, user.FullName, user.Role, user.Email } });
        }

        // 3. ĐỔI MẬT KHẨU
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // Kiểm tra mật khẩu cũ có đúng không
            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            {
                return BadRequest(new { message = "Mật khẩu hiện tại không đúng" });
            }

            // Mã hóa và lưu mật khẩu mới
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }

        // 4. ADMIN: LẤY DANH SÁCH USER
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto(u.Id, u.Username, u.FullName, u.Email, u.Role))
                .ToListAsync();
            return Ok(users);
        }
        
        // 5. ADMIN: ĐỔI QUYỀN USER
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleRole(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            
            if (user.Username == "admin") return BadRequest(new { message = "Không thể thay đổi quyền của Admin gốc" });

            user.Role = user.Role == "Admin" ? "User" : "Admin";
            await _context.SaveChangesAsync();
            
            return Ok(new { message = $"Đã đổi quyền của {user.Username} thành {user.Role}" });
        }
    }
}