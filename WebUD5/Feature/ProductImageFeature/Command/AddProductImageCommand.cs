using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.ProductImageFeature.Command
{
    public class AddProductImageCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class AddProductImageCommandHandler : IRequestHandler<AddProductImageCommand, int>
    {
        private readonly WebUD5DbContext _context;

        public AddProductImageCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Sản phẩm không tồn tại");

            var productImage = new ProductImage
            {
                ProductId = request.ProductId,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return productImage.Id;
        }
    }

}
