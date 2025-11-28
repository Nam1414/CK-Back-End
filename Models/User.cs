using System.ComponentModel.DataAnnotations;// <--- Quan trọng: Dòng này để dùng [Required], [EmailAddress], etc.

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty; // Lưu password đã hash
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // "Admin" hoặc "User"
    public string? ResetToken { get; set; } // Lưu mã OTP
    public DateTime? ResetTokenExpiry { get; set; } // Lưu thời gian hết hạ
}