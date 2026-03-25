using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.AuthFeature.Command
{
    using MediatR;
    using WebUD5.Models;
    using Microsoft.EntityFrameworkCore;
    using WebUD5.Service;

    public class UpdateProfileCommand : IRequest<AuthResponse>
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string ImgProfile { get; set; }
    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, AuthResponse>
    {
        private readonly WebUD5DbContext _context;
        private readonly IJwtService _jwtService;

        public UpdateProfileCommandHandler(WebUD5DbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
                throw new Exception("User not found.");

            user.FullName = request.FullName ?? user.FullName;
            user.Gender = request.Gender ?? user.Gender;
            user.Phone = request.Phone ?? user.Phone;
            user.Address = request.Address ?? user.Address;
            user.Birthday = request.Birthday ?? user.Birthday;
            user.ImgProfile = request.ImgProfile ?? user.ImgProfile;

            await _context.SaveChangesAsync(cancellationToken);

            var token = _jwtService.GenerateToken(user);
            return new AuthResponse(user, token);
        }
    }

}
