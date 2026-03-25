using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.ProductFeature.Command
{
    public class AddProductCommand : IRequest<int>
    {
        public int SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    // Handler
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly WebUD5DbContext _context;

        public AddProductCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                SellerId = request.SellerId,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Status = "pending", // Sản phẩm mới thêm cần được duyệt
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id; // Trả về ID của sản phẩm vừa thêm
        }
    }
}
