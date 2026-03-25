using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;
using WebUD5.Service;

namespace WebUD5.Feature.AuthFeature.Command
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly WebUD5DbContext _context;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(WebUD5DbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user == null)
                throw new Exception("User not found.");

            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendResetPasswordEmail(user.Email, user.ResetToken);
            return true;
        }
    }

}
