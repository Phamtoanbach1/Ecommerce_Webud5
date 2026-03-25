using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.PaymentFeature.Command
{
    public class AddPaymentCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }

        public AddPaymentCommand(int orderId, int userId, string paymentMethod, decimal amount)
        {
            OrderId = orderId;
            UserId = userId;
            PaymentMethod = paymentMethod;
            Amount = amount;
        }
    }

    public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public AddPaymentCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                OrderId = request.OrderId,
                UserId = request.UserId,
                PaymentMethod = request.PaymentMethod,
                Amount = request.Amount,
                Status = "pending",
                PaymentDate = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
