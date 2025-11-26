public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; } // Liên kết với người dùng đăng nhập
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Status { get; set; } = "pending";
    public decimal TotalAmount { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = new();
}