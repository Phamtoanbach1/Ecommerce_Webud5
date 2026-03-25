using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUD5.Feature.ProductFeature.Command;
using WebUD5.Feature.ProductFeature.Queries;
using WebUD5.Feature.ProductImageFeature.Command;
using WebUD5.Feature.ProductImageFeature.Queries;

namespace WebUD5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProductImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductImage([FromBody] AddProductImageCommand command)
        {
            var imageId = await _mediator.Send(command);
            return Ok(new { ImageId = imageId, Message = "Thêm ảnh thành công!" });
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteProductImage(int imageId)
        {
            var result = await _mediator.Send(new DeleteProductImageCommand { ImageId = imageId });
            if (!result) return NotFound("Ảnh không tồn tại!");
            return Ok(new { Message = "Xóa ảnh thành công!" });
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductImages(int productId)
        {
            var images = await _mediator.Send(new GetProductImagesQuery { ProductId = productId });
            return Ok(images);
        }
    }
}
