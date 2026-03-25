using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.OrderFeature.Queries
{
    public class GetOrderDetailQuery : IRequest<OrderDetailDto>
    {
        public int OrderId { get; set; }

        public GetOrderDetailQuery(int orderId)
        {
            OrderId = orderId;
        }
    }

    public class GetOrderDetailQueryHandler : IRequestHandler<GetOrderDetailQuery, OrderDetailDto>
    {
        private readonly WebUD5DbContext _context;

        public GetOrderDetailQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<OrderDetailDto> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.Shipper)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
                return null;

            return new OrderDetailDto
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User.FullName,
                ShipperId = order.ShipperId,
                ShipperName = order.Shipper?.FullName,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                DeliveryStatus = order.DeliveryStatus,
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }
    }

}
