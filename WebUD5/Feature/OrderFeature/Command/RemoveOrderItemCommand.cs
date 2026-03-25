using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;

namespace WebUD5.Feature.OrderFeature.Command
{
    public class RemoveOrderItemCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public RemoveOrderItemCommand(int orderId, int productId)
        {
            OrderId = orderId;
            ProductId = productId;
        }
    }

    public class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public RemoveOrderItemCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null) return false;

            var orderItem = order.OrderItems.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (orderItem == null) return false;

            // Xóa sản phẩm khỏi đơn hàng
            _context.OrderItems.Remove(orderItem);

            // Cập nhật tổng giá trị đơn hàng
            order.TotalPrice = order.OrderItems.Where(i => i.ProductId != request.ProductId)
                                               .Sum(i => i.Quantity * i.Price);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
