using MediatR;
using WebUD5.Models;

namespace WebUD5.Feature.UserFeature.Command
{
    public class UpdateUserStatusCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public bool Status { get; set; }

        public UpdateUserStatusCommand(int id, bool status)
        {
            Id = id;
            Status = status;
        }
    }

    public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, bool>
    {
        private readonly WebUD5DbContext _context;

        public UpdateUserStatusCommandHandler(WebUD5DbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.Id);

            if (user == null) return false;

            user.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
