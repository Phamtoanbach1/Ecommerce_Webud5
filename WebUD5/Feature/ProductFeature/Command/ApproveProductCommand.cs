using MediatR;
using System;
using WebUD5.Models;

namespace WebUD5.Feature.ProductFeature.Command
{
    public class ApproveProductCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public string Status { get; set; } // "approved" hoặc "rejected"
    }
    public class ApproveProductCommandHandler : IRequestHandler<ApproveProductCommand, bool>
    {
        private readonly WebUD5DbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApproveProductCommandHandler(WebUD5DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(ApproveProductCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (!user.IsInRole("admin") && !user.IsInRole("seller"))
            {
                throw new UnauthorizedAccessException("Bạn không có quyền duyệt sản phẩm.");
            }

            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }

            product.Status = request.Status.ToLower() switch
            {
                "approved" => "approved",
                "rejected" => "rejected",
                _ => throw new ArgumentException("Trạng thái không hợp lệ.")
            };

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
