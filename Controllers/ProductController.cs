using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPICS.Contracts;
using TodoAPICS.Interfaces;

namespace TodoAPICS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Viewer")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productServices.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Viewer")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productServices.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductDto dto)
        {
            var created = await _productServices.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] ProductDto dto)
        {
            var updated = await _productServices.UpdateProductAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _productServices.DeleteProductAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("category/{category}")]
        [Authorize(Roles = "Admin,Viewer")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetByCategory(string category)
        {
            var products = await _productServices.GetProductsByCategoryAsync(category);
            return Ok(products);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Viewer")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Search([FromQuery] string term)
        {
            var results = await _productServices.SearchProductsAsync(term);
            return Ok(results);
        }

    }
}