using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Data;
using ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // USER + ADMIN
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        // ADMIN
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Tên sản phẩm không được rỗng");

            if (req.Price < 0)
                return BadRequest("Giá phải >= 0");

            if (req.Stock < 0)
                return BadRequest("Tồn kho phải >= 0");

            var product = new Product
            {
                Name = req.Name,
                Price = req.Price,
                Stock = req.Stock,
                Description = req.Description
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
        }

        // ADMIN
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateRequest req)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Tên sản phẩm không được rỗng");

            if (req.Price < 0)
                return BadRequest("Giá phải >= 0");

            if (req.Stock < 0)
                return BadRequest("Tồn kho phải >= 0");

            product.Name = req.Name;
            product.Price = req.Price;
            product.Stock = req.Stock;
            product.Description = req.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ADMIN
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}