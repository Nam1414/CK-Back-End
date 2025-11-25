namespace ProductAPI.Models
{
   public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;

    // Mật khẩu phải hash
    public string PasswordHash { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    // Default role
    public string Role { get; set; } = "user";

    // Quên mật khẩu
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpire { get; set; }
}

}