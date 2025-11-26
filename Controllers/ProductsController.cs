using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.DTOs;   // <-- Quan trọng
using OrderManagementAPI.Entity; // <-- Quan trọng

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProductsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    [Authorize(Roles = "Admin")] // Chỉ Admin mới được thêm sửa xóa
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(ProductCreateDto dto)
    {
        var product = new Product { Name = dto.Name, Price = dto.Price, Stock = dto.Stock };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }
    
    // Thêm các hàm PUT, DELETE tương tự...
}