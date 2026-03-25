using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using WebUD5.Models;

namespace WebUD5.Feature.WishlistFeature.Command
{
    public class AddToWishlistCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public AddToWishlistCommand(int userId, int productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }

    public class AddToWishlistCommandHandler : IRequestHandler<AddToWishlistCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public AddToWishlistCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra xem sản phẩm có tồn tại không
            var productExists = await _context.Products.AnyAsync(p => p.Id == request.ProductId);
            if (!productExists)
            {
                throw new ArgumentException("Sản phẩm không tồn tại.");
            }

            // Kiểm tra xem sản phẩm đã có trong wishlist chưa
            var existingWishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == request.UserId && w.ProductId == request.ProductId);

            if (existingWishlistItem != null)
            {
                throw new InvalidOperationException("Sản phẩm đã có trong danh sách yêu thích.");
            }

            // Thêm sản phẩm vào wishlist
            var wishlistItem = new Wishlist
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
