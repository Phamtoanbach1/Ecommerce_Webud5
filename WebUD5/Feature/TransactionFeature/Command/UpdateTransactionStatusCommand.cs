using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.TransactionFeature.Command
{
    public class UpdateTransactionStatusCommand : IRequest<bool>
    {
        public int TransactionId { get; set; }
        public string Status { get; set; } // "pending", "success", "failed", "refunded"

        public UpdateTransactionStatusCommand(int transactionId, string status)
        {
            TransactionId = transactionId;
            Status = status;
        }
    }

    public class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransactionStatusCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public UpdateTransactionStatusCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.FindAsync(request.TransactionId);
            if (transaction == null) return false;

            transaction.TransactionStatus = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
