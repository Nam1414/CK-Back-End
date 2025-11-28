namespace OrderManagementAPI.Services // <--- Định nghĩa interface IEmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message); // Phương thức gửi email bất đồng bộ
    }
}