using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.ReviewFeature.Queries
{
    public class GetProductReviewsQuery : IRequest<List<ReviewDto>>
    {
        public int ProductId { get; set; }
    }

    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, List<ReviewDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetProductReviewsQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewDto>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == request.ProductId)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }

}
