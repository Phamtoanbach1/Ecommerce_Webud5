using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.ProductImageFeature.Command
{
    public class DeleteProductImageCommand : IRequest<bool>
    {
        public int ImageId { get; set; }
    }

    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public DeleteProductImageCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _context.ProductImages.FindAsync(request.ImageId);
            if (image == null)
                throw new KeyNotFoundException("Ảnh không tồn tại");

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
