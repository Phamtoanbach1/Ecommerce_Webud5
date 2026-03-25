using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.UserFeature.Command
{
    public class ChangeUserRoleCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string NewRole { get; set; } // 'customer', 'seller', 'admin', 'shipper'

        public ChangeUserRoleCommand(int id, string newRole)
        {
            Id = id;
            NewRole = newRole;
        }
    }

    public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public ChangeUserRoleCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null) return false;

            // Kiểm tra role hợp lệ
            var validRoles = new List<string> { "customer", "seller", "admin", "shipper" };
            if (!validRoles.Contains(request.NewRole.ToLower()))
            {
                throw new ArgumentException("Invalid role provided.");
            }

            user.Role = request.NewRole.ToLower(); // Cập nhật role
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }


}
