using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Services;
using ProductModel = ProductAPI.Models.Product;  // ← THÊM DÒNG NÀY (alias)

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(DataStore.Products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = DataStore.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm" });
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateProduct([FromBody] ProductModel product)  // ← ĐỔI Product → ProductModel
        {
            if (string.IsNullOrEmpty(product.Name))
                return BadRequest(new { message = "Tên sản phẩm không được rỗng" });
            if (product.Price < 0)
                return BadRequest(new { message = "Giá phải lớn hơn hoặc bằng 0" });
            if (product.Stock < 0)
                return BadRequest(new { message = "Tồn kho phải lớn hơn hoặc bằng 0" });

            product.Id = DataStore.NextProductId++;
            DataStore.Products.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductModel product)  // ← ĐỔI Product → ProductModel
        {
            var existingProduct = DataStore.Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm" });

            if (string.IsNullOrEmpty(product.Name))
                return BadRequest(new { message = "Tên sản phẩm không được rỗng" });
            if (product.Price < 0)
                return BadRequest(new { message = "Giá phải lớn hơn hoặc bằng 0" });
            if (product.Stock < 0)
                return BadRequest(new { message = "Tồn kho phải lớn hơn hoặc bằng 0" });

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.Stock = product.Stock;
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            var product = DataStore.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm" });

            DataStore.Products.Remove(product);
            return NoContent();
        }
    }
}