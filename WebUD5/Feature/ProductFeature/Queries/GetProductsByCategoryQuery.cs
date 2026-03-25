using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.ProductFeature.Queries
{
    public class GetProductsByCategoryQuery : IRequest<List<ProductDto>>
    {
        public int CategoryId { get; set; }

        public GetProductsByCategoryQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, List<ProductDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetProductsByCategoryQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == request.CategoryId && p.Status == "approved")
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedAt = p.CreatedAt,
                    SellerId = p.SellerId,
                    CategoryId = p.CategoryId
                })
                .ToListAsync(cancellationToken);

            return products;
        }
    }

}
