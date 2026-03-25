using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.PaymentFeature.Queries
{
    public class GetPaymentsByUserQuery : IRequest<List<PaymentDto>>
    {
        public int UserId { get; set; }

        public GetPaymentsByUserQuery(int userId)
        {
            UserId = userId;
        }
    }

    public class GetPaymentsByUserQueryHandler : IRequestHandler<GetPaymentsByUserQuery, List<PaymentDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetPaymentsByUserQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentDto>> Handle(GetPaymentsByUserQuery request, CancellationToken cancellationToken)
        {
            var payments = await _context.Payments
                .Where(p => p.UserId == request.UserId)
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    OrderId = p.OrderId,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync(cancellationToken);

            return payments;
        }
    }

}
