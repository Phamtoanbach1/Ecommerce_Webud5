using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUD5.Feature.PaymentFeature.Command;
using WebUD5.Feature.PaymentFeature.Queries;
using WebUD5.Service;

namespace WebUD5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly VNPayService _VNPayService;

        public PaymentController(IMediator mediator, VNPayService VNPayService)
        {
            _mediator = mediator;
            _VNPayService = VNPayService;
        }

        private readonly string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private readonly string returnUrl = $"http://localhost:{4200}/callback";
        private readonly string tmnCode = "PBE1R802";
        private readonly string hashSecret = "L5LOIXX2HK3UVHHBJLLGLO65JESM39YW";

        [HttpGet("Payment")]
        public IActionResult Payment([FromQuery] string amount, [FromQuery] string infor)
        {
            string hostName = System.Net.Dns.GetHostName();
            string clientIPAddress = System.Net.Dns.GetHostAddresses(hostName).FirstOrDefault()?.ToString();

            var txnref = Guid.NewGuid();

            _VNPayService.AddRequestData("vnp_Version", "2.1.0");
            _VNPayService.AddRequestData("vnp_Command", "pay");
            _VNPayService.AddRequestData("vnp_TmnCode", tmnCode);
            _VNPayService.AddRequestData("vnp_Amount", (int.Parse(amount) * 100).ToString());
            _VNPayService.AddRequestData("vnp_BankCode", "");
            _VNPayService.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            _VNPayService.AddRequestData("vnp_CurrCode", "VND");
            _VNPayService.AddRequestData("vnp_IpAddr", clientIPAddress);
            _VNPayService.AddRequestData("vnp_Locale", "vn");
            _VNPayService.AddRequestData("vnp_OrderInfo", infor);
            _VNPayService.AddRequestData("vnp_OrderType", "other");
            _VNPayService.AddRequestData("vnp_ReturnUrl", returnUrl);
            _VNPayService.AddRequestData("vnp_TxnRef", txnref.ToString());

            string paymentUrl = _VNPayService.CreateRequestUrl(url, hashSecret);
            return Ok(new { paymentUrl });
        }


        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] AddPaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return result ? Ok("Payment added successfully") : BadRequest("Failed to add payment");
        }

        // Lấy danh sách Payment theo UserId
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUser(int userId)
        {
            var result = await _mediator.Send(new GetPaymentsByUserQuery(userId));
            return Ok(result);
        }

        // Cập nhật trạng thái Payment
        [HttpPut("{paymentId}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentId,string status)
        {
            var result = await _mediator.Send(new UpdatePaymentStatusCommand(paymentId, status));
            return result ? Ok("Payment status updated") : NotFound("Payment not found");
        }
    }
}
