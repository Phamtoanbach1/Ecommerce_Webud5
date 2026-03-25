using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.ProductFeature.Queries
{
    public class GetProductDetailQuery : IRequest<ProductDto>
    {
        public int ProductId { get; set; }

        public GetProductDetailQuery(int productId)
        {
            ProductId = productId;
        }
    }
    public class GetProductDetailQueryHandler : IRequestHandler<GetProductDetailQuery, ProductDto>
    {
        private readonly WebUD5DbContext _context;

        public GetProductDetailQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Status = product.Status,
                CategoryName = product.Category.Name,
                SellerName = product.Seller.FullName,
                ImageUrls = product.ProductImages.Select(img => img.ImageUrl).ToList()
            };
        }
    }

}
