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
    [Authorize] // Yêu cầu người dùng phải đăng nhập mới được truy cập API trong controller này
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Khởi tạo controller với dependency injection của DbContext
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Lấy thông tin cá nhân của user hiện đang đăng nhập
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            // Lấy userId từ claim NameIdentifier trong token
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            // Tìm user trong database theo userId
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound(); // Nếu không tìm thấy trả về 404

            // Trả về thông tin cơ bản của user
            return Ok(new 
            { 
                user.Id, user.Username, user.FullName, user.Email, user.Role ,
                user.PhoneNumber
            });
        }

        // 2. Cập nhật thông tin cá nhân (Tên đầy đủ, Email, số điện thoại)
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // Nếu đổi email, kiểm tra xem email mới đã tồn tại trong database chưa
            if (user.Email != dto.Email && await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest(new { message = "Email này đã được sử dụng bởi tài khoản khác" });
            }

            // Cập nhật thông tin user
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();

            // Trả về thông báo cập nhật thành công và dữ liệu user mới
            return Ok(new { message = "Cập nhật thông tin thành công", user = new { user.Id, user.Username, user.FullName, user.Role, user.Email } });
        }

        // 3. Đổi mật khẩu cho user hiện tại
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // Kiểm tra mật khẩu cũ có đúng không (dùng BCrypt để so sánh)
            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            {
                return BadRequest(new { message = "Mật khẩu hiện tại không đúng" });
            }

            // Mã hóa mật khẩu mới và cập nhật
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }

        // 4. ADMIN: Lấy danh sách tất cả user trong hệ thống
        [HttpGet]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới được gọi API này
        public async Task<IActionResult> GetAllUsers()
        {
            // Lấy danh sách user và map sang DTO để trả về
            var users = await _context.Users
                .Select(u => new UserDto(u.Id, u.Username, u.FullName, u.Email, u.Role))
                .ToListAsync();
            return Ok(users);
        }
        
        // 5. ADMIN: Đổi quyền (role) của một user, chuyển giữa Admin và User
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới được đổi quyền user
        public async Task<IActionResult> ToggleRole(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Không cho đổi quyền của admin gốc
            if (user.Username == "admin") return BadRequest(new { message = "Không thể thay đổi quyền của Admin gốc" });

            // Đổi quyền kiểu toggling giữa "Admin" và "User"
            user.Role = user.Role == "Admin" ? "User" : "Admin";
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã đổi quyền của {user.Username} thành {user.Role}" });
        }
    }
}
