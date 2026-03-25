using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.UserFeature.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public int UserId { get; set; }

        public GetUserByIdQuery() { }
        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly WebUD5DbContext _context;

        public GetUserByIdQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(u => u.Id == request.UserId && u.Status)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    EmailVerified = u.EmailVerified,
                    Phone = u.Phone,
                    Address = u.Address,
                    Birthday = u.Birthday,
                    ImgProfile = u.ImgProfile,
                    Role = u.Role,
                    Status = u.Status,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("User not found or account is disabled.");

            return user;
        }
    }

}
