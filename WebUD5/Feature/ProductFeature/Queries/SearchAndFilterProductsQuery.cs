using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.ProductFeature.Queries
{
    public class SearchAndFilterProductsQuery : IRequest<List<ProductDto>>
    {
        public string? Keyword { get; set; }  // Từ khóa tìm kiếm
        public int? CategoryId { get; set; }  // Lọc theo danh mục
        public decimal? MinPrice { get; set; }  // Giá tối thiểu
        public decimal? MaxPrice { get; set; }  // Giá tối đa
        public string? Status { get; set; }  // Trạng thái sản phẩm: pending, approved, rejected
        public bool? InStock { get; set; }  // Chỉ lấy sản phẩm còn hàng hay không
    }

    public class SearchAndFilterProductsQueryHandler : IRequestHandler<SearchAndFilterProductsQuery, List<ProductDto>>
    {
        private readonly WebUD5DbContext _context;

        public SearchAndFilterProductsQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(SearchAndFilterProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products.AsQueryable();

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(p => p.Name.Contains(request.Keyword));
            }

            // Lọc theo danh mục
            if (request.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == request.CategoryId);
            }

            // Lọc theo giá
            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice);
            }
            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice);
            }

            // Lọc theo trạng thái sản phẩm
            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(p => p.Status == request.Status.ToLower());
            }

            // Lọc theo hàng tồn kho
            if (request.InStock.HasValue)
            {
                query = query.Where(p => request.InStock.Value ? p.StockQuantity > 0 : p.StockQuantity == 0);
            }

            var products = await query
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
                .ToListAsync();

            return products;
        }
    }

}
