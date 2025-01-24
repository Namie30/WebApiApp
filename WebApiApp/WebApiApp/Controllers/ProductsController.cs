using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;

namespace WebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllProducts());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound(new { Message = $"Product with ID {id} not found." });
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await _productService.GetProductById(product.Id);
            if (existingProduct == null)
                return NotFound(new { Message = $"Product with ID {product.Id} not found." });

            await _productService.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryId(categoryId);
            return Ok(products);
        }

        [HttpGet("category/{categoryId}/total-price")]
        public async Task<IActionResult> GetTotalPriceByCategory(int categoryId)
        {
            var totalPrice = await _productService.GetTotalPriceByCategoryId(categoryId);
            return Ok(new { CategoryId = categoryId, TotalPrice = totalPrice });
        }

        [HttpGet("total-price-per-category")]
        public async Task<IActionResult> GetTotalPricePerCategory()
        {
            var totalPrices = await _productService.GetTotalPricePerCategory();
            return Ok(totalPrices);
        }
    }
}
