using MediatR;
using WebUD5.Models.DTOs;
using WebUD5.Models;
using WebUD5.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace WebUD5.Feature.AuthFeature.Command
{
    public class LoginWithGoogleCommand : IRequest<string> { }

    public class LoginWithGoogleCommandHandler : IRequestHandler<LoginWithGoogleCommand, string>
    {
        private readonly WebUD5DbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginWithGoogleCommandHandler(WebUD5DbContext context, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
        {
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded) throw new Exception("Google login failed");

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var fullName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email)) throw new Exception("Email not received");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
            {
                // Tạo số điện thoại Việt Nam ngẫu nhiên
                string[] prefixes = { "09", "08", "07", "03", "05" };
                string randomPrefix = prefixes[new Random().Next(prefixes.Length)];
                string randomNumber = new Random().Next(10000000, 99999999).ToString();
                string generatedPhone = randomPrefix + randomNumber;

                // Tạo username từ email
                string generatedUsername = email.Split('@')[0];

                user = new User
                {
                    Username = generatedUsername,
                    Email = email,
                    FullName = fullName ?? "No Name",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), // Mật khẩu giả
                    Phone = generatedPhone,
                    Address = "N/A",
                    Gender = null,
                    Birthday = null,
                    ImgProfile = null,
                    EmailVerified = true, // Google đã xác thực email
                    VerificationToken = null,
                    Role = "customer",
                    CreatedAt = DateTime.Now,
                    Status = true,
                    ResetToken = null,
                    ResetTokenExpiry = null
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var token = _jwtService.GenerateToken(user);
            return token;
        }
    }

}
