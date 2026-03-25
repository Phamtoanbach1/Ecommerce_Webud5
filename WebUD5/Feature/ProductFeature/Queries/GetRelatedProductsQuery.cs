using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.ProductFeature.Queries
{
    public class GetRelatedProductsQuery : IRequest<List<ProductDto>>
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int Limit { get; set; } = 5; // Mặc định lấy 5 sản phẩm

        public GetRelatedProductsQuery(int productId, int categoryId, int limit = 5)
        {
            ProductId = productId;
            CategoryId = categoryId;
            Limit = limit;
        }
    }
    public class GetRelatedProductsQueryHandler : IRequestHandler<GetRelatedProductsQuery, List<ProductDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetRelatedProductsQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetRelatedProductsQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                return new List<ProductDto>(); // Trả về danh sách rỗng nếu không tìm thấy sản phẩm
            }

            var relatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != request.ProductId)
                .OrderByDescending(p => p.CreatedAt)
                .Take(request.Limit)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    SellerId = p.SellerId,
                    CategoryId = p.CategoryId,
                    SellerName = p.Seller.FullName,
                    CategoryName = p.Category.Name,
                    ImageUrls = p.ProductImages.Select(img => img.ImageUrl).ToList() // Lấy danh sách ảnh
                })
                .ToListAsync(cancellationToken);

            return relatedProducts;
        }
    }
}
