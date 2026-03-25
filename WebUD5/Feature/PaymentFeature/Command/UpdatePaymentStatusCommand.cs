using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.PaymentFeature.Command
{
    public class UpdatePaymentStatusCommand : IRequest<bool>
    {
        public int PaymentId { get; set; }
        public string Status { get; set; } // "pending", "completed", "failed", "refunded"

        public UpdatePaymentStatusCommand(int paymentId, string status)
        {
            PaymentId = paymentId;
            Status = status;
        }
    }

    public class UpdatePaymentStatusCommandHandler : IRequestHandler<UpdatePaymentStatusCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public UpdatePaymentStatusCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FindAsync(request.PaymentId);
            if (payment == null) return false;

            payment.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
