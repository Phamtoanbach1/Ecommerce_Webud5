using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUD5.Feature.WishlistFeature.Command;
using WebUD5.Feature.WishlistFeature.Queries;

namespace WebUD5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WishlistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            int userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            var command = new AddToWishlistCommand(userId, productId);
            var result = await _mediator.Send(command);

            return result ? Ok("Sản phẩm đã được thêm vào danh sách yêu thích.")
                          : BadRequest("Không thể thêm sản phẩm vào danh sách yêu thích.");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            var result = await _mediator.Send(new GetWishlistQuery(userId));
            return Ok(result);
        }
    }
}
