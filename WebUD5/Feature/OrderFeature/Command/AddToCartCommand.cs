using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using WebUD5.Models;

namespace WebUD5.Feature.CartFeature.Command
{
    public class AddToCartCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public AddToCartCommand(int userId, int productId, int quantity)
        {
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public AddToCartCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null || product.StockQuantity < request.Quantity)
            {
                throw new ArgumentException("Sản phẩm không tồn tại hoặc không đủ hàng.");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == request.UserId && o.Status == "pending");

            if (order == null)
            {
                order = new Order
                {
                    UserId = request.UserId,
                    TotalPrice = 0,
                    Status = "pending",
                    CreatedAt = DateTime.UtcNow
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == order.Id && oi.ProductId == request.ProductId);

            if (orderItem != null)
            {
                orderItem.Quantity += request.Quantity;
            }
            else
            {
                orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = product.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            order.TotalPrice += request.Quantity * product.Price;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
