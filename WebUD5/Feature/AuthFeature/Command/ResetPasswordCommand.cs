using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.AuthFeature.Command
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public ResetPasswordCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == request.Token && u.ResetTokenExpiry > DateTime.UtcNow, cancellationToken);
            if (user == null)
                throw new Exception("Invalid or expired token.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
