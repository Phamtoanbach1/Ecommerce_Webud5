using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;

namespace WebUD5.Feature.UserFeature.Command
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string ImgProfile { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public UpdateUserCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId && u.Status, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException("User not found or account is disabled.");

            user.FullName = request.FullName ?? user.FullName;
            user.Phone = request.Phone ?? user.Phone;
            user.Address = request.Address ?? user.Address;
            user.Birthday = request.Birthday ?? user.Birthday;
            user.ImgProfile = request.ImgProfile ?? user.ImgProfile;

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}
