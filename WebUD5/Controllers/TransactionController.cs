using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUD5.Feature.TransactionFeature.Command;
using WebUD5.Feature.TransactionFeature.Queries;

namespace WebUD5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            return result ? Ok("Transaction added successfully") : BadRequest("Failed to add transaction");
        }

        [HttpGet("payment/{paymentId}")]
        public async Task<IActionResult> GetTransactionsByPayment(int paymentId)
        {
            var result = await _mediator.Send(new GetTransactionsByPaymentQuery(paymentId));
            return Ok(result);
        }

        [HttpPut("{transactionId}/status")]
        public async Task<IActionResult> UpdateTransactionStatus(int transactionId, string status)
        {
            var result = await _mediator.Send(new UpdateTransactionStatusCommand(transactionId, status));
            return result ? Ok("Transaction status updated") : NotFound("Transaction not found");
        }
    }
}
