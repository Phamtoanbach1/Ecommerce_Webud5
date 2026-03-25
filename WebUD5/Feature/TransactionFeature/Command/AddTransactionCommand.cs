using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.TransactionFeature.Command
{
    public class AddTransactionCommand : IRequest<bool>
    {
        public int PaymentId { get; set; }
        public string TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string? ResponseMessage { get; set; }

        public AddTransactionCommand(int paymentId, string transactionCode, decimal amount, string transactionStatus, string? responseMessage)
        {
            PaymentId = paymentId;
            TransactionCode = transactionCode;
            Amount = amount;
            TransactionStatus = transactionStatus;
            ResponseMessage = responseMessage;
        }
    }


    public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public AddTransactionCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                PaymentId = request.PaymentId,
                TransactionCode = request.TransactionCode,
                Amount = request.Amount,
                TransactionStatus = request.TransactionStatus,
                CreatedAt = DateTime.UtcNow,
                ResponseMessage = request.ResponseMessage
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
