using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.TransactionFeature.Queries
{
    public class GetTransactionsByPaymentQuery : IRequest<List<TransactionDto>>
    {
        public int PaymentId { get; set; }

        public GetTransactionsByPaymentQuery(int paymentId)
        {
            PaymentId = paymentId;
        }
    }

    public class GetTransactionsByPaymentQueryHandler : IRequestHandler<GetTransactionsByPaymentQuery, List<TransactionDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetTransactionsByPaymentQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionDto>> Handle(GetTransactionsByPaymentQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _context.Transactions
                .Where(t => t.PaymentId == request.PaymentId)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    PaymentId = t.PaymentId,
                    TransactionCode = t.TransactionCode,
                    Amount = t.Amount,
                    TransactionStatus = t.TransactionStatus,
                    CreatedAt = t.CreatedAt,
                    ResponseMessage = t.ResponseMessage
                })
                .ToListAsync(cancellationToken);

            return transactions;
        }
    }

}
