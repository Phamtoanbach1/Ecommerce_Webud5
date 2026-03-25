using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUD5.Feature.AuthFeature.Command;
using WebUD5.Feature.UserFeature.Command;
using WebUD5.Feature.UserFeature.Queries;

namespace WebUD5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var query = new GetUserByIdQuery { UserId = id }; 
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound(new { message = "User not found" });

            return Ok(result);
        }

        /// <summary>
        /// Cập nhật thông tin cá nhân của user (chỉ cập nhật chính user đang đăng nhập)
        /// </summary>
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            //int? userIdFromToken = GetUserIdFromClaims();
            //if (userIdFromToken == null || userIdFromToken != command.UserId)
            //    return Forbid();

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Admin cập nhật thông tin user khác
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin user hiện tại từ token
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            int? userIdFromToken = GetUserIdFromClaims();
            if (userIdFromToken == null)
                return Unauthorized(new { message = "Invalid token" });

            var query = new GetUserByIdQuery { UserId = userIdFromToken.Value };
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound(new { message = "User not found" });
        }

        [HttpPost("request-role-change")]
        public async Task<IActionResult> RequestRoleChange([FromBody] RequestRoleChangeCommand command)
        {
            var result = await _mediator.Send(command);
            return result ? Ok("Request submitted successfully") : BadRequest("Failed to submit request");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("approve-role-change")]
        public async Task<IActionResult> ApproveRoleChange([FromBody] ApproveRoleChangeCommand command)
        {
            var result = await _mediator.Send(command);
            return result ? Ok("Role request processed") : BadRequest("Failed to process request");
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> ChangeUserRole(int id, [FromBody] ChangeUserRoleCommand command)
        {
            if (id != command.Id) return BadRequest();

            var result = await _mediator.Send(command);
            return result ? Ok(new { message = "User role updated successfully." }) : NotFound();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UpdateUserStatusCommand command)
        {
            if (id != command.Id) return BadRequest();
            var result = await _mediator.Send(command);
            return result ? Ok() : NotFound();
        }


        /// <summary>
        /// Lấy userId từ JWT Token
        /// </summary>
        private int? GetUserIdFromClaims()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}
