using System.Threading;
using System.Threading.Tasks;

namespace Paye.Application.Contracts
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
        Task<Result> RegisterAsync(string username, string password, string email, string role, CancellationToken cancellationToken = default);
        Task<Result> ChangePasswordAsync(string username, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
        Task LogoutAsync(CancellationToken cancellationToken = default);
    }
}
