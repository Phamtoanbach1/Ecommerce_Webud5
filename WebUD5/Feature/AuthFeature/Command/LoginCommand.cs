using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUD5.Models;
using WebUD5.Models.DTOs;
using WebUD5.Service;

namespace WebUD5.Feature.AuthFeature.Command
{
    public class LoginCommand : IRequest<AuthResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly WebUD5DbContext _context;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(WebUD5DbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email || u.Username == request.Email, cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.EmailVerified)
                throw new UnauthorizedAccessException("Email is not verified. Please verify your email first.");

            if (!user.Status)
                throw new UnauthorizedAccessException("Your account has been disabled. Please contact support.");

            var token = _jwtService.GenerateToken(user);
            return new AuthResponse(user, token);
        }
    }

}
  