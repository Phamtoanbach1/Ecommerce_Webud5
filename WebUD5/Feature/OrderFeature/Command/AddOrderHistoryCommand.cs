using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;

namespace WebUD5.Feature.OrderFeature.Command
{
    public class AddOrderHistoryCommand : IRequest<bool>
    {
        public int OrderId { get; set; }

        public AddOrderHistoryCommand(int orderId)
        {
            OrderId = orderId;
        }
    }

    public class AddOrderHistoryCommandHandler : IRequestHandler<AddOrderHistoryCommand, bool>
    {
        private readonly WebUD5DbContext _context;
        private const string InitialStatus = "Order Placed";

        public AddOrderHistoryCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddOrderHistoryCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"🔍 Nhận OrderId: {request.OrderId}");

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

                if (order == null)
                {
                    Console.WriteLine("⚠️ Không tìm thấy đơn hàng trong database.");
                    return false;
                }

                // Kiểm tra xem đơn hàng đã có trong OrderHistory chưa
                var existingHistoryCount = await _context.OrderHistories
                    .CountAsync(oh => oh.OrderId == order.Id, cancellationToken);

                Console.WriteLine($"🔍 Số bản ghi trong OrderHistories: {existingHistoryCount}");

                if (existingHistoryCount > 0)
                {
                    Console.WriteLine("⚠️ Order đã có lịch sử, không thêm lại.");
                    return false;
                }

                // Thêm sản phẩm vào lịch sử đơn hàng
                var orderHistories = order.OrderItems.Select(item => new OrderHistory
                {
                    UserId = order.UserId,
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Status = InitialStatus,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                await _context.OrderHistories.AddRangeAsync(orderHistories, cancellationToken);

                // Cập nhật trạng thái đơn hàng
                order.Status = "pending";
                order.DeliveryStatus = "Pending";

                await _context.SaveChangesAsync(cancellationToken);
                Console.WriteLine("✅ Lưu lịch sử đơn hàng thành công.");
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                Console.WriteLine($"❌ Lỗi khi thêm Order History: {ex.Message}");
                return false;
            }
        }
    }



}
