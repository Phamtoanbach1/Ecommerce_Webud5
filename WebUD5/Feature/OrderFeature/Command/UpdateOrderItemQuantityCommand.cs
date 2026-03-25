using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;

namespace WebUD5.Feature.OrderFeature.Command
{
    public class UpdateOrderItemQuantityCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public UpdateOrderItemQuantityCommand(int orderId, int productId, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

    public class UpdateOrderItemQuantityCommandHandler : IRequestHandler<UpdateOrderItemQuantityCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public UpdateOrderItemQuantityCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateOrderItemQuantityCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
            {
                return false; // Không cho phép số lượng về 0
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null) return false;

            var orderItem = order.OrderItems.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (orderItem == null) return false;

            // Cập nhật số lượng sản phẩm trong đơn hàng
            orderItem.Quantity = request.Quantity;

            // Cập nhật tổng giá trị đơn hàng
            order.TotalPrice = order.OrderItems.Sum(i => i.Quantity * i.Price);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
