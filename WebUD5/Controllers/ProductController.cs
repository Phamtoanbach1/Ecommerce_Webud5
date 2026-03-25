using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUD5.Feature.ProductFeature.Command;
using WebUD5.Feature.ProductFeature.Queries;
using WebUD5.Feature.ReviewFeature.Command;

namespace WebUD5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "seller")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductCommand command)
        {
            var productId = await _mediator.Send(command);
            if (productId > 0)
                return Ok(new { Message = "Tạo sản phẩm thành công!", ProductId = productId });
            return BadRequest("Không thể tạo sản phẩm.");
        }

        [Authorize(Roles = "seller")]
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok("Cập nhật sản phẩm thành công!");
            return BadRequest("Cập nhật sản phẩm thất bại.");
        }

        [Authorize(Roles = "seller,admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            if (!result)
            {
                return BadRequest("Failed to delete product.");
            }
            return Ok("Product deleted successfully.");
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _mediator.Send(new GetProductsByCategoryQuery(categoryId));
            return Ok(products);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetProductDetails(int id)
        {
            var product = await _mediator.Send(new GetProductDetailQuery(id));

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        [Authorize(Roles = "admin,seller")]
        [HttpPut("{productId}/approve")]
        public async Task<IActionResult> ApproveProduct(int productId, [FromBody] ApproveProductCommand command)
        {
            if (productId != command.ProductId)
            {
                return BadRequest("Product ID không khớp.");
            }

            var result = await _mediator.Send(command);
            return result ? Ok("Sản phẩm đã được cập nhật trạng thái.") : BadRequest("Cập nhật trạng thái thất bại.");
        }



        [HttpGet("search")]
        public async Task<IActionResult> SearchAndFilterProducts(
        [FromQuery] string? keyword,
        [FromQuery] int? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string? status,
        [FromQuery] bool? inStock)
        {
            var query = new SearchAndFilterProductsQuery
            {
                Keyword = keyword,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Status = status,
                InStock = inStock
            };

            var products = await _mediator.Send(query);
            return Ok(products);
        }
        [HttpGet("{productId}/related")]
        public async Task<IActionResult> GetRelatedProducts(int productId, [FromQuery] int limit = 5)
        {
            var product = await _mediator.Send(new GetProductDetailQuery(productId));
            if (product == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại." });
            }

            var query = new GetRelatedProductsQuery(productId, product.CategoryId, limit);
            var relatedProducts = await _mediator.Send(query);

            return Ok(relatedProducts);
        }
    }
}
