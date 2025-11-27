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
[Authorize] // Bắt buộc phải đăng nhập mới gọi được
public async Task<IActionResult> GetOrders()
{
    // 1. Lấy thông tin người đang đăng nhập từ Token
    var role = User.FindFirst(ClaimTypes.Role)?.Value; // Ví dụ: "Admin" hoặc "User"
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Ví dụ: 1, 2...

    if (userIdClaim == null) return Unauthorized();
    int currentUserId = int.Parse(userIdClaim.Value);

    // 2. Tạo câu truy vấn cơ bản (chưa chạy ngay)
    var query = _context.Orders
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .AsQueryable(); // Dùng AsQueryable để có thể nối thêm điều kiện WHERE

    // 3. QUAN TRỌNG NHẤT: PHÂN QUYỀN DỮ LIỆU
    // Nếu người dùng KHÔNG PHẢI là Admin...
    if (role != "Admin")
    {
        // ...Thì chỉ được lấy những đơn hàng có UserId trùng với ID của họ.
        // Điều này có nghĩa là đơn của Admin (UserId=1) hay đơn của User khác (UserId=3) sẽ bị loại bỏ ngay lập tức.
        query = query.Where(o => o.UserId == currentUserId);
    }

    // 4. Thực thi truy vấn và trả về kết quả
    // Lúc này SQL mới thực sự chạy và chỉ lấy về đúng dữ liệu được phép xem.
    var orders = await query.ToListAsync();

    return Ok(orders);
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
[Authorize]
public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
{
    // 1. Tìm đơn hàng trong Database
    var order = await _context.Orders.FindAsync(id);
    if (order == null) return NotFound(new { message = "Không tìm thấy đơn hàng" });

    // 2. Lấy thông tin người đang đăng nhập từ Token
    var role = User.FindFirst(ClaimTypes.Role)?.Value; // Lấy quyền (Admin/User)
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Lấy User ID
    
    if (userIdClaim == null) return Unauthorized();
    int currentUserId = int.Parse(userIdClaim.Value);

    // 3. LOGIC BẢO MẬT (QUAN TRỌNG NHẤT)
    // Nếu KHÔNG PHẢI Admin...
    if (role != "Admin")
    {
        // ...Thì kiểm tra xem đơn hàng này có phải của chính họ không?
        if (order.UserId != currentUserId)
        {
            return StatusCode(403, new { message = "Bạn không có quyền can thiệp vào đơn hàng của người khác!" });
        }

        // ...Và User thường chỉ được phép "Hủy" (cancelled), không được phép tự "Duyệt" (processing) hay "Hoàn thành" (completed)
        if (status.ToLower() != "cancelled")
        {
            return StatusCode(403, new { message = "Khách hàng chỉ có quyền Hủy đơn hàng." });
        }

        // ...Và chỉ được hủy khi đơn đang "pending" (chưa được shop xử lý)
        if (order.Status.ToLower() != "pending")
        {
            return BadRequest(new { message = "Không thể hủy đơn hàng đã được xử lý hoặc đang giao." });
        }
    }

    // 4. Cập nhật trạng thái
    order.Status = status;
    
    // (Tùy chọn) Nếu là Hủy đơn -> Cộng lại tồn kho
    if (status.ToLower() == "cancelled")
    {
        // Logic hoàn kho (cần load OrderDetails trước nếu muốn làm kỹ)
        // Hiện tại ta chỉ đổi trạng thái cho đơn giản
    }

    await _context.SaveChangesAsync();
    return Ok(new { message = "Cập nhật trạng thái thành công" });
}
// --- THÊM ĐOẠN NÀY VÀO ORDERSCONTROLLER.CS ---

// API Xóa đơn hàng (Chỉ Admin được xóa)
[HttpDelete("{id}")]
[Authorize(Roles = "Admin")] 
public async Task<IActionResult> DeleteOrder(int id)
{
    // 1. Tìm đơn hàng cần xóa (kèm cả chi tiết để xóa sạch)
    var order = await _context.Orders
                              .Include(o => o.OrderDetails)
                              .FirstOrDefaultAsync(o => o.Id == id);

    // 2. Nếu không tìm thấy trong Database -> Báo lỗi 404
    if (order == null) 
    {
        return NotFound(new { message = $"Không tìm thấy đơn hàng ID = {id}" });
    }

    // 3. (Tùy chọn) Hoàn trả tồn kho trước khi xóa
    // Nếu đơn chưa hoàn thành và chưa hủy, trả lại hàng vào kho
    if (order.Status != "completed" && order.Status != "cancelled")
    {
        foreach (var item in order.OrderDetails)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null) 
            {
                product.Stock += item.Quantity;
            }
        }
    }

    // 4. Xóa khỏi Database
    _context.Orders.Remove(order);
    await _context.SaveChangesAsync();

    return Ok(new { message = "Đã xóa đơn hàng thành công" });
}
}