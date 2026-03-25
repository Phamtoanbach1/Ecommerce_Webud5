namespace WebUD5.Models.DTOs
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string ImgProfile { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
        public string Token { get; set; }

        public AuthResponse(User user, string token)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            EmailVerified = user.EmailVerified;
            Phone = user.Phone;
            Address = user.Address;
            Birthday = user.Birthday;
            ImgProfile = user.ImgProfile;
            Role = user.Role;
            Status = user.Status;
            Token = token;
        }
    }

}
