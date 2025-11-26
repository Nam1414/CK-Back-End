using System.ComponentModel.DataAnnotations;

namespace OrderManagementAPI.DTOs
{
    // 1. DTO cho phần Đăng nhập/Đăng ký
    public record LoginDto(string Username, string Password);
    
    public record RegisterDto(string Username, string Password, string FullName);
    
    // Thêm DTO trả về khi login thành công (để dùng trong AuthController)
    public record AuthResponseDto(string Token, string Username, string FullName, string Role);

    // 2. DTO cho phần Sản phẩm
    public record ProductCreateDto(
        [Required] string Name, 
        [Range(0, double.MaxValue)] decimal Price, 
        int Stock,
        string Description // Tôi bổ sung thêm Description cho khớp với Model
    );

    // 3. DTO cho phần Đơn hàng
    public record OrderItemDto(int ProductId, int Quantity);
    
    public record CreateOrderDto(
        string CustomerName, 
        string CustomerPhone, 
        string CustomerAddress, 
        List<OrderItemDto> Items
    );
}