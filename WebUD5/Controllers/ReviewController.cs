using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUD5.Feature.ReviewFeature.Command;
using WebUD5.Feature.ReviewFeature.Queries;

namespace WebUD5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
        {
            if(User.FindFirst(ClaimTypes.NameIdentifier)?.Value != null)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                command.UserId = userId;
            }
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest("Không thể thêm đánh giá.");
            }

            return Ok(new { message = "Đánh giá đã được thêm thành công." });
        }

        [HttpDelete("{reviewId}")]
        [Authorize(Roles = "admin,seller")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var command = new DeleteReviewCommand { ReviewId = reviewId };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound("Không thể xóa đánh giá.");
            }

            return Ok(new { message = "Đánh giá đã được xóa thành công." });
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            var query = new GetProductReviewsQuery { ProductId = productId };
            var reviews = await _mediator.Send(query);
            return Ok(reviews);
        }
    }
}
