using System.ComponentModel.DataAnnotations; // <--- Quan trọng: Dòng này sửa lỗi [Required]

namespace OrderManagementAPI.DTOs
{
    // --- NHÓM AUTH (Đăng nhập/Đăng ký/Quên mật khẩu) ---
    
    public record LoginDto(string Username, string Password);

    public record RegisterDto(
        [Required] string Username, 
        [Required] string Password, 
        [Required] string FullName,
        [Required, EmailAddress] string Email // <--- Đã thêm trường Email
    );
    // 1. DTO Cập nhật thông tin (Chỉ còn Tên và Email)
    public record UpdateProfileDto(
        [Required] string FullName,
        [Required, EmailAddress] string Email
    );

    // 2. DTO Đổi mật khẩu
    public record ChangePasswordDto(
        [Required] string CurrentPassword,
        [Required] string NewPassword
    );
    
    // 3. DTO Danh sách User (Cho Admin xem)
    public record UserDto(int Id, string Username, string FullName, string Email, string Role);

    public record AuthResponseDto(string Token, string Username, string FullName, string Role);

    // DTO cho quên mật khẩu (Tìm bằng Email)
    public record ForgotPasswordDto([Required, EmailAddress] string Email);

    // DTO cho đặt lại mật khẩu (Xác định user bằng Email)
    public record ResetPasswordDto(string Email, string Otp, string NewPassword);


    // --- NHÓM SẢN PHẨM & ĐƠN HÀNG ---

    public record ProductCreateDto(
        [Required] string Name, 
        [Range(0, double.MaxValue)] decimal Price, 
        int Stock,
        string Description
    );

    public record OrderItemDto(int ProductId, int Quantity);

    public record CreateOrderDto(
        string CustomerName, 
        string CustomerPhone, 
        string CustomerAddress, 
        List<OrderItemDto> Items
    );
}