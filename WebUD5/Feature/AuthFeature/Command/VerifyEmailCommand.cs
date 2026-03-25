using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;

namespace WebUD5.Feature.AuthFeature.Command
{
    public class VerifyEmailCommand : IRequest<bool>
    {
        public string Token { get; set; }
    }

    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public VerifyEmailCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == request.Token, cancellationToken);
            if (user == null)
                throw new Exception("Invalid or expired token.");

            user.EmailVerified = true;
            user.VerificationToken = null;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
