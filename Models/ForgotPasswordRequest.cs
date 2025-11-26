namespace ProductAPI.Models
{
public class ForgotPasswordRequest
{
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
}