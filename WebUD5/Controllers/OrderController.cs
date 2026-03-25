using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUD5.Feature.CartFeature.Command;
using WebUD5.Feature.OrderFeature.Command;
using WebUD5.Feature.OrderFeature.Queries;

namespace WebUD5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            int userId = int.Parse(User.FindFirst("id")?.Value ?? "3");
            var command = new AddToCartCommand(userId, productId, quantity);
            var result = await _mediator.Send(command);
            return result ? Ok("Thêm vào giỏ hàng thành công.") : BadRequest("Không thể thêm vào giỏ hàng.");
        }

        [HttpGet("user/orderhistory/{userId}")]
        public async Task<IActionResult> GetOrderHistory(int userId)
        {
            var query = new GetOrderHistoryQuery(userId);
            var result = await _mediator.Send(query);

            if (result == null || result.Count == 0)
                return NotFound("Không tìm thấy lịch sử đơn hàng nào.");

            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddOrderHistory([FromBody] AddOrderHistoryCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Thêm lịch sử đơn hàng thất bại.");

            return Ok("Lịch sử đơn hàng đã được thêm thành công.");
        }

        [HttpDelete("{orderId}/items/{productId}")]
        public async Task<IActionResult> RemoveOrderItem(int orderId, int productId)
        {
            var command = new RemoveOrderItemCommand(orderId, productId);
            var result = await _mediator.Send(command);
            return result ? Ok(new { message = "Product removed from order." }) : NotFound();
        }

        [HttpPut("{orderId}/items/{productId}")]
        public async Task<IActionResult> UpdateOrderItemQuantity(int orderId, int productId, int quantity)
        {
            var command = new UpdateOrderItemQuantityCommand(orderId, productId, quantity);
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest(new { message = "Invalid quantity. Must be greater than 0." });

            return Ok(new { message = "Order item updated successfully." });
        }

        [HttpGet("detail/{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var query = new GetOrderDetailQuery(orderId);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound("Không tìm thấy đơn hàng.");

            return Ok(result);
        }

        [HttpGet("user/order/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var result = await _mediator.Send(new GetOrdersByUserQuery(userId));
            return Ok(result);
        }
    }
}
