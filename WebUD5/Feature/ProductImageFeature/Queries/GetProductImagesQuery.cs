using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.ProductImageFeature.Queries
{
    public class GetProductImagesQuery : IRequest<List<ProductImageDto>>
    {
        public int ProductId { get; set; }
    }

    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQuery, List<ProductImageDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetProductImagesQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductImageDto>> Handle(GetProductImagesQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProductImages
                .Where(pi => pi.ProductId == request.ProductId)
                .Select(pi => new ProductImageDto
                {
                    Id = pi.Id,
                    ImageUrl = pi.ImageUrl,
                    CreatedAt = pi.CreatedAt
                })
                .ToListAsync();
        }
    }

}
