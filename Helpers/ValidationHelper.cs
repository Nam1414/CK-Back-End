using ProductAPI.Models;
using System.Linq;

namespace ProductAPI.Helpers
{
    public static class ValidationHelper
    {
        // LOGIN
        public static string? ValidateLogin(LoginRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.Username)) 
                return "Username không được để trống";

            if (string.IsNullOrWhiteSpace(r.Password)) 
                return "Mật khẩu không được để trống";

            return null;
        }

        // REGISTER
        public static string? ValidateRegister(RegisterRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.Username)) 
                return "Username không được để trống";

            if (string.IsNullOrWhiteSpace(r.FullName)) 
                return "Họ tên không được để trống";

            if (string.IsNullOrWhiteSpace(r.Password)) 
                return "Mật khẩu không được để trống";

            if (r.Password.Length < 6) 
                return "Mật khẩu tối thiểu 6 ký tự";

            if (r.Password != r.ConfirmPassword) 
                return "Mật khẩu xác nhận không khớp";

            return null;
        }

        // FORGOT PASSWORD
        public static string? ValidateForgot(ForgotPasswordRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.Email)) 
                return "Email không được để trống";

            if (!r.Email.Contains("@")) 
                return "Email không đúng định dạng";

            return null;
        }

        // RESET PASSWORD
        public static string? ValidateReset(ResetPasswordRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.Email)) return "Email không được để trống";
            if (string.IsNullOrWhiteSpace(r.Otp)) return "OTP không hợp lệ";
            if (string.IsNullOrWhiteSpace(r.NewPassword)) return "Mật khẩu mới không được để trống";
            if (r.NewPassword.Length < 6) return "Mật khẩu tối thiểu 6 ký tự";
            return null;
        }
    }
}
