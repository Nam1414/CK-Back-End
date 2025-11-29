using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.DTOs;   // <-- Dùng để lấy các lớp DTO truyền dữ liệu
using OrderManagementAPI.Entity; // <-- Dùng để lấy các lớp thực thể (entity) ánh xạ bảng cơ sở dữ liệu


[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context; // Khai báo biến DbContext để thao tác với database

    // Constructor nhận dependency injection DbContext từ service container
    public ProductsController(AppDbContext context) => _context = context;


    // API GET: /api/products
    // Lấy danh sách toàn bộ sản phẩm trong cơ sở dữ liệu
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync(); // Trả về danh sách sản phẩm dưới dạng bất đồng bộ
    }


    // API POST: /api/products
    // Yêu cầu quyền Admin mới được gọi để thêm sản phẩm mới vào database
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(ProductCreateDto dto)
    {
        // Tạo một đối tượng product mới từ dữ liệu dto nhận vào
        var product = new Product { Name = dto.Name, Price = dto.Price, Stock = dto.Stock };

        _context.Products.Add(product); // Thêm product mới vào DbSet để EF theo dõi
        await _context.SaveChangesAsync(); // Lưu thay đổi xuống cơ sở dữ liệu

        // Trả về kết quả thành công với status 201 Created, đồng thời trả về product đã thêm
        // và link dẫn tới action GetProducts để lấy danh sách sản phẩm (thường là GetProduct theo id sẽ hợp lý hơn)
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }
    
    // Có thể thêm các hàm PUT, DELETE tương tự để sửa và xóa sản phẩm...
}
