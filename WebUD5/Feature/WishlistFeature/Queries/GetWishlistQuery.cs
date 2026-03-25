using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.WishlistFeature.Queries
{
    public class GetWishlistQuery : IRequest<List<ProductDto>>
    {
        public int UserId { get; set; }

        public GetWishlistQuery(int userId)
        {
            UserId = userId;
        }
    }
    public class GetWishlistQueryHandler : IRequestHandler<GetWishlistQuery, List<ProductDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetWishlistQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
        {
            var wishlist = await _context.Wishlists
                .Where(w => w.UserId == request.UserId)
                .Select(w => new ProductDto
                {
                    Id = w.Product.Id,
                    Name = w.Product.Name,
                    Price = w.Product.Price,
                    ImageUrls = w.Product.ProductImages.Select(i => i.ImageUrl).ToList()
                })
                .ToListAsync(cancellationToken);

            return wishlist;
        }
    }

}
