using MediatR;
using Paye.Application.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Paye.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<AuthResult>
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterCommand : IRequest<Result>
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class ChangePasswordCommand : IRequest<Result>
    {
        public string UserName { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class LogoutCommand : IRequest<Result> { }
}
