using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OrderManagementAPI.DTOs;   // Quan trọng: Dùng các DTO để nhận dữ liệu đầu vào
using OrderManagementAPI.Entity; // Quan trọng: Entity định nghĩa model DB

[Route("api/[controller]")]
[ApiController]
[Authorize] // Tất cả API phía dưới đều yêu cầu đăng nhập
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    public OrdersController(AppDbContext context) => _context = context;

    [HttpGet]
    [Authorize] // Chỉ người đăng nhập mới lấy được đơn hàng
    public async Task<IActionResult> GetOrders()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value; 
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();
        int currentUserId = int.Parse(userIdClaim.Value);

        var query = _context.Orders
                            .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Product)
                            .AsQueryable();

        // QUAN TRỌNG: Phân quyền dữ liệu - User thường chỉ xem đơn hàng của mình
        if (role != "Admin")
        {
            query = query.Where(o => o.UserId == currentUserId);
        }

        var orders = await query.ToListAsync();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("Token không hợp lệ");
        int userId = int.Parse(userIdClaim.Value);

        var order = new Order
        {
            UserId = userId,
            CustomerName = dto.CustomerName,
            CustomerPhone = dto.CustomerPhone,
            CustomerAddress = dto.CustomerAddress,
            CreatedAt = DateTime.Now,
            Status = "pending",  // Trạng thái mặc định
            TotalAmount = 0,
            OrderDetails = new List<OrderDetail>()
        };

        foreach (var item in dto.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null) 
                return BadRequest(new { message = $"Sản phẩm ID {item.ProductId} không tồn tại" });
            if (product.Stock < item.Quantity) 
                return BadRequest(new { message = $"Sản phẩm '{product.Name}' không đủ hàng (Còn: {product.Stock})" });

            product.Stock -= item.Quantity; // Trừ tồn kho

            var detail = new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };
            order.OrderDetails.Add(detail);
            order.TotalAmount += detail.Quantity * detail.UnitPrice;
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return StatusCode(201, order);
    }

    [HttpPut("{id}/status")]
    [Authorize]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound(new { message = "Không tìm thấy đơn hàng" });

        var role = User.FindFirst(ClaimTypes.Role)?.Value; 
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();
        int currentUserId = int.Parse(userIdClaim.Value);

        // QUAN TRỌNG: Kiểm soát quyền hạn cập nhật trạng thái đơn
        if (role != "Admin")
        {
            if (order.UserId != currentUserId)
                return StatusCode(403, new { message = "Bạn không có quyền can thiệp vào đơn hàng của người khác!" });

            // User thường chỉ được phép Hủy đơn hàng (trạng thái "cancelled")
            if (status.ToLower() != "cancelled")
                return StatusCode(403, new { message = "Khách hàng chỉ có quyền Hủy đơn hàng." });

            // Chỉ được hủy đơn khi đơn ở trạng thái "pending"
            if (order.Status.ToLower() != "pending")
                return BadRequest(new { message = "Không thể hủy đơn hàng đã được xử lý hoặc đang giao." });
        }

        order.Status = status;

        // Nếu hủy đơn, có thể thêm logic hoàn kho ở đây (không triển khai hiện tại)
        
        await _context.SaveChangesAsync();
        return Ok(new { message = "Cập nhật trạng thái thành công" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Chỉ Admin mới được xóa đơn hàng
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders
                                  .Include(o => o.OrderDetails)
                                  .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) 
            return NotFound(new { message = $"Không tìm thấy đơn hàng ID = {id}" });

        // Hoàn trả tồn kho nếu đơn chưa hoàn thành hoặc chưa bị hủy
        if (order.Status != "completed" && order.Status != "cancelled")
        {
            foreach (var item in order.OrderDetails)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null) 
                    product.Stock += item.Quantity;
            }
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Đã xóa đơn hàng thành công" });
    }
}
