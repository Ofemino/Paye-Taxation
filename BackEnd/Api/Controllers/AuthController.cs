using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paye.Api.Models;
using Paye.Application.Features.Auth.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

using Paye.Shared;

namespace Paye.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new LoginCommand { UserName = loginDto.UserName, Password = loginDto.Password }, cancellationToken);
            if (!result.Success)
                return Unauthorized(ApiResponse.Fail(result.Error));
            return Ok(ApiResponse.Succeed(new { Token = result.Token }));
        }

        [HttpPost("register")]
        [Authorize(Roles = "IT Operations,Tax Unit")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RegisterCommand { UserName = registerDto.UserName, Password = registerDto.Password, Email = registerDto.Email, Role = registerDto.Role }, cancellationToken);
            if (!result.Success)
                return BadRequest(ApiResponse.Fail(result.Error));
            return Ok(ApiResponse.Succeed(message: "Registration successful"));
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken cancellationToken)
        {
            // Users can only change their own password, or admins can change any password
            var currentUserName = User.Identity?.Name;
            var isAdmin = User.IsInRole("IT Operations") || User.IsInRole("Tax Unit");
            
            if (!isAdmin && currentUserName != dto.UserName)
            {
                return Forbid("You can only change your own password.");
            }

            var result = await _mediator.Send(new ChangePasswordCommand { UserName = dto.UserName, CurrentPassword = dto.CurrentPassword, NewPassword = dto.NewPassword }, cancellationToken);
            if (!result.Success)
                return BadRequest(ApiResponse.Fail(result.Error));
            return Ok(ApiResponse.Succeed(message: "Password changed successfully"));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> Logout(CancellationToken cancellationToken)
        {
            await _mediator.Send(new LogoutCommand(), cancellationToken);
            return Ok(ApiResponse.Succeed(message: "Logged out successfully"));
        }
    }
}
