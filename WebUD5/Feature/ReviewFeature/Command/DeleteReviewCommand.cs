using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.ReviewFeature.Command
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public int ReviewId { get; set; }
    }

    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public DeleteReviewCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews.FindAsync(request.ReviewId);
            if (review == null)
            {
                throw new KeyNotFoundException("Đánh giá không tồn tại.");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
