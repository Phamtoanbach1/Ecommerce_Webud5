using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using WebUD5.Models;

namespace WebUD5.Feature.ProductFeature.Command
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Status { get; set; } // 'pending', 'approved', 'rejected'
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public UpdateProductCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.SellerId == request.SellerId, cancellationToken);

            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại hoặc không thuộc quyền sở hữu của bạn.");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
