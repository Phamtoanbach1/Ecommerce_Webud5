using SendGrid.Helpers.Mail;
using SendGrid;

namespace WebUD5.Service
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string email, string token);
        Task SendResetPasswordEmail(string email, string token);
    }

    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _ownerMail;
        private readonly string _companyName;

        public EmailService(IConfiguration config)
        {
            _apiKey = config["EmailApiKey:Key"];
            _ownerMail = config["EmailApiKey:OwnerMail"];
            _companyName = config["EmailApiKey:Company"];

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new ArgumentNullException(nameof(_apiKey), "SendGrid API key is missing in configuration.");
            }
        }

        public async Task SendVerificationEmail(string email, string token)
        {
            var client = new SendGridClient(_apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_ownerMail, _companyName),
                Subject = "Verify Your Email",
                PlainTextContent = $"Click the link to verify: http://localhost:5223/api/auth/verify-email?token={token}",
                HtmlContent = $"<p>Click <a href='http://localhost:5223/api/auth/verify-email?token={token}'>here</a> to verify.</p>"

            };
            msg.AddTo(new EmailAddress(email));

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to send verification email.");
            }
        }

        public async Task SendResetPasswordEmail(string email, string token)
        {
            var client = new SendGridClient(_apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_ownerMail, _companyName),
                Subject = "Reset Your Password",
                PlainTextContent = $"Click the link to reset password: http://localhost:5223/api/auth/reset-password?token={token}",
                HtmlContent = $"<p>Click <a href='http://localhost:5223/api/auth/reset-password?token={token}'>here</a> to reset password.</p>"
            };
            msg.AddTo(new EmailAddress(email));

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to send reset password email.");
            }
        }
    }

}
