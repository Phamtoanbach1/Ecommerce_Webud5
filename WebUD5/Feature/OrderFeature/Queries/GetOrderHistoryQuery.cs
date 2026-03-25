using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.OrderFeature.Queries
{
    public class GetOrderHistoryQuery : IRequest<List<OrderHistoryDto>>
    {
        public int UserId { get; set; }

        public GetOrderHistoryQuery(int userId)
        {
            UserId = userId;
        }
    }

    public class GetOrderHistoryQueryHandler : IRequestHandler<GetOrderHistoryQuery, List<OrderHistoryDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetOrderHistoryQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderHistoryDto>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
        {
            var orderHistories = await _context.OrderHistories
                .Where(oh => oh.UserId == request.UserId)
                .OrderByDescending(oh => oh.CreatedAt)
                .Select(oh => new OrderHistoryDto
                {
                    OrderId = oh.OrderId,
                    ProductId = oh.ProductId,
                    ProductName = oh.Product.Name,
                    Quantity = oh.Quantity,
                    Price = oh.Price,
                    Status = oh.Status,
                    CreatedAt = oh.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return orderHistories;
        }
    }

}
