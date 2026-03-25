using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;

namespace WebUD5.Feature.ReviewFeature.Command
{
    public class CreateReviewCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public int UserId { get; set; } 
        public int Rating { get; set; } 
        public string? Comment { get; set; }
    }

    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public CreateReviewCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new ArgumentException("Xếp hạng phải từ 1 đến 5.");
            }

            var productExists = await _context.Products.AnyAsync(p => p.Id == request.ProductId, cancellationToken);
            if (!productExists)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }

            var review = new Review
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
