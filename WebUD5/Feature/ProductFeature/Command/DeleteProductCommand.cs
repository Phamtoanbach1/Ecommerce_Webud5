using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.ProductFeature.Command
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int ProductId { get; set; }

        public DeleteProductCommand(int productId)
        {
            ProductId = productId;
        }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public DeleteProductCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
