using MediatR;
using WebUD5.Models.DTOs;
using WebUD5.Models;
using Microsoft.EntityFrameworkCore;

namespace WebUD5.Feature.UserFeature.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>> { }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly WebUD5DbContext _context;

        public GetAllUsersQueryHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
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
                .ToListAsync(cancellationToken);
        }
    }

}
