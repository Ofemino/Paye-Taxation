using MediatR;
using Paye.Application.Contracts;
using Paye.Application.Features.Auth.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Paye.Application.Features.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly IAuthService _authService;
        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _authService.LoginAsync(request.UserName, request.Password, cancellationToken);
        }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IAuthService _authService;
        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return _authService.RegisterAsync(request.UserName, request.Password, request.Email, request.Role, cancellationToken);
        }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IAuthService _authService;
        public ChangePasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return _authService.ChangePasswordAsync(request.UserName, request.CurrentPassword, request.NewPassword, cancellationToken);
        }
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IAuthService _authService;
        public LogoutCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync(cancellationToken);
            return new Result { Success = true };
        }
    }
}
