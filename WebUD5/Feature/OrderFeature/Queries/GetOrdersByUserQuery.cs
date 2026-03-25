using MediatR;
using WebUD5.Models.DTOs;
using WebUD5.Models;
using Microsoft.EntityFrameworkCore;

namespace WebUD5.Feature.OrderFeature.Queries
{
    public class GetOrdersByUserQuery : IRequest<List<OrderDto>>
    {
        public int UserId { get; set; }

        public GetOrdersByUserQuery(int userId)
        {
            UserId = userId;
        }
    }
    public class GetOrdersByUserQueryHandler : IRequestHandler<GetOrdersByUserQuery, List<OrderDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetOrdersByUserQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == request.UserId)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    DeliveryStatus = o.DeliveryStatus,
                    CreatedAt = o.CreatedAt,
                    Items = o.OrderItems.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return orders;
        }
    }

}
