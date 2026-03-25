namespace WebUD5.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string ImgProfile { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
