using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;
using WebUD5.Service;

namespace WebUD5.Feature.AuthFeature.Command
{
    public class RegisterCommand : IRequest<bool>
    {
        public string Username {  get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly WebUD5DbContext _context;
        private readonly IEmailService _emailService;

        public RegisterCommandHandler(WebUD5DbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
                throw new Exception("Email already exists.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var verificationToken = Guid.NewGuid().ToString();

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Username = request.Username,
                PasswordHash = hashedPassword,
                Phone = request.Phone,
                EmailVerified = false,
                VerificationToken = verificationToken,
                Status = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendVerificationEmail(user.Email, verificationToken);
            return true;
        }
    }

}
