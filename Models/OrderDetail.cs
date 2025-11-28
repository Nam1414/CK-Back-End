public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;//truy cập thông tin đơn hàng cha
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;//truy cập thông tin sản phẩm
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}