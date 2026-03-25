using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.UserFeature.Command
{
    public class ApproveRoleChangeCommand : IRequest<bool>
    {
        public int RequestId { get; set; }
        public bool IsApproved { get; set; } // true = approved, false = rejected
    }
    public class ApproveRoleChangeHandler : IRequestHandler<ApproveRoleChangeCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public ApproveRoleChangeHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ApproveRoleChangeCommand request, CancellationToken cancellationToken)
        {
            var roleRequest = await _context.RequestManagements.FindAsync(request.RequestId);
            if (roleRequest == null || roleRequest.Status != "pending")
                throw new Exception("Invalid or already processed request.");

            var user = await _context.Users.FindAsync(roleRequest.UserId);
            if (user == null)
                throw new Exception("User not found.");

            if (request.IsApproved)
            {
                // Lấy vai trò từ yêu cầu đã lưu
                var requestedRole = roleRequest.RequestedRole;
                if (requestedRole != "seller" && requestedRole != "shipper")
                    throw new Exception("Invalid role request.");

                user.Role = requestedRole; // Gán role theo yêu cầu của user
                roleRequest.Status = "approved";
                roleRequest.ApprovedAt = DateTime.UtcNow;
            }
            else
            {
                roleRequest.Status = "rejected";
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
