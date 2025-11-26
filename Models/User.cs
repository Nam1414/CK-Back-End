namespace ProductAPI.Models
{
   public class User
{
 public int Id { get; set; }

        public string Username { get; set; } =string.Empty;
        public string FullName { get; set; } =string.Empty;

        public string PasswordHash { get; set; } =string.Empty;
        public string Role { get; set; } = "user";

        // Dùng cho quên mật khẩu
        public string? ResetToken { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email {get; set;} 

        public DateTime? ResetTokenExpire { get; set; }
}

}