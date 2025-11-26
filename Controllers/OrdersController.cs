using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OrderManagementAPI.DTOs;   // <-- Quan trọng
using OrderManagementAPI.Entity; // <-- Quan trọng
[Route("api/[controller]")]
[ApiController]
[Authorize] // Bắt buộc đăng nhập
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    public OrdersController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        var query = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).AsQueryable();

        // Nếu không phải Admin, chỉ xem đơn của chính mình
        if (role != "Admin")
        {
            query = query.Where(o => o.UserId == userId);
        }

        return Ok(await query.ToListAsync());
    }

    [HttpPost]
   public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
{
    // 1. Lấy ID người dùng từ Token
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null) return Unauthorized("Token không hợp lệ");
    int userId = int.Parse(userIdClaim.Value);
    
    // 2. Tạo đối tượng Order
    var order = new Order
    {
        UserId = userId,
        CustomerName = dto.CustomerName,
        CustomerPhone = dto.CustomerPhone,
        CustomerAddress = dto.CustomerAddress,
        CreatedAt = DateTime.Now,
        
        // 👇 QUAN TRỌNG: Phải viết thường chữ "pending"
        Status = "pending", 
        
        TotalAmount = 0,
        OrderDetails = new List<OrderDetail>()
    };

    // 3. Xử lý từng sản phẩm
    foreach (var item in dto.Items)
    {
        var product = await _context.Products.FindAsync(item.ProductId);
        
        // Kiểm tra tồn tại
        if (product == null) 
            return BadRequest(new { message = $"Sản phẩm ID {item.ProductId} không tồn tại" });
        
        // Kiểm tra tồn kho
        if (product.Stock < item.Quantity) 
            return BadRequest(new { message = $"Sản phẩm '{product.Name}' không đủ hàng (Còn: {product.Stock})" });

        // Trừ kho
        product.Stock -= item.Quantity;
        
        // Tạo chi tiết đơn hàng
        var detail = new OrderDetail
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = product.Price
        };
        
        order.OrderDetails.Add(detail);
        order.TotalAmount += detail.Quantity * detail.UnitPrice;
    }

    // 4. Lưu vào Database
    _context.Orders.Add(order);
    await _context.SaveChangesAsync();

    // Trả về kết quả (201 Created)
    return StatusCode(201, order);
}
    [HttpPut("{id}/status")]
public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
{
    var order = await _context.Orders.FindAsync(id);
    if (order == null) return NotFound();

    // Logic kiểm tra: Chỉ Admin hoặc chính chủ mới được hủy
    var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
    var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);

    if (role != "Admin" && order.UserId != userId) return Forbid();

    order.Status = status;
    await _context.SaveChangesAsync();
    return Ok(new { message = "Cập nhật thành công" });
}
[HttpDelete("{id}")]
[Authorize(Roles = "Admin")] // Chỉ Admin mới được xóa
public async Task<IActionResult> DeleteOrder(int id)
{
    var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);
    if (order == null) return NotFound();

    // Hoàn trả tồn kho nếu xóa đơn chưa hoàn thành (Optional)
    if (order.Status != "completed" && order.Status != "cancelled")
    {
        foreach (var item in order.OrderDetails)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null) product.Stock += item.Quantity;
        }
    }

    _context.Orders.Remove(order);
    await _context.SaveChangesAsync();
    return Ok(new { message = "Đã xóa đơn hàng" });
}
}