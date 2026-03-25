using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.UserFeature.Command
{
    public class RequestRoleChangeCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string RequestedRole { get; set; } // "seller" hoặc "shipper"
    }
    public class RequestRoleChangeHandler : IRequestHandler<RequestRoleChangeCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public RequestRoleChangeHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(RequestRoleChangeCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null || user.Role != "customer")
                throw new Exception("Invalid user or user is not a customer.");

            if (request.RequestedRole != "seller" && request.RequestedRole != "shipper")
                throw new Exception("Invalid role request.");

            var roleRequest = new RequestManagement
            {
                UserId = request.UserId,
                RequestedRole = request.RequestedRole,
                Status = "pending",
                RequestDate = DateTime.UtcNow
            };

            _context.RequestManagements.Add(roleRequest);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
