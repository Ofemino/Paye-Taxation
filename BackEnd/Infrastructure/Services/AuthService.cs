using System.Threading.Tasks;
using Paye.Application.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading;

using Microsoft.Extensions.Options;
using Paye.Infrastructure.Configuration;

namespace Paye.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IExternalAuthProvider _externalAuthProvider;
        private readonly AuthenticationSettings _authSettings;
        private readonly ExternalAuthSettings _externalAuthSettings;

        public AuthService(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, 
            IConfiguration config,
            IExternalAuthProvider externalAuthProvider,
            IOptions<AuthenticationSettings> authSettings,
            IOptions<ExternalAuthSettings> externalAuthSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _externalAuthProvider = externalAuthProvider;
            _authSettings = authSettings.Value;
            _externalAuthSettings = externalAuthSettings.Value;
        }

        public async Task<AuthResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            // Check if External Auth is preferred
            if (_authSettings.UseExternal)
            {
                var externalResult = await _externalAuthProvider.ValidateUserAsync(username, password, cancellationToken);
                
                if (externalResult != null && externalResult.IsValid)
                {
                    // External validation succeeded
                    var user = await _userManager.FindByNameAsync(username);
                    
                    if (user == null && _externalAuthSettings.AutoCreateUsers)
                    {
                        // Auto-create user
                        user = new IdentityUser { UserName = externalResult.Username, Email = externalResult.Email };
                        var createResult = await _userManager.CreateAsync(user); // Password not needed for external users
                        if (!createResult.Succeeded)
                             return new AuthResult { Success = false, Error = "Failed to create local user from external profile." };
                    }
                    else if (user == null)
                    {
                        return new AuthResult { Success = false, Error = "User not found locally and auto-creation is disabled." };
                    }

                    // Sync roles if enabled
                    if (_externalAuthSettings.AutoSyncRoles && externalResult.Roles != null)
                    {
                         var currentRoles = await _userManager.GetRolesAsync(user);
                         await _userManager.RemoveFromRolesAsync(user, currentRoles);
                         await _userManager.AddToRolesAsync(user, externalResult.Roles);
                    }

                    return await GenerateJwtTokenAsync(user);
                }
                // Fallback to local if external fails? Or just fail?
                // Usually if UseExternal is true, we expect it to be the source of truth.
                // However, user might have meant "Try external, then local". 
                // Given the requirement "UseExternal: false" implies a switch, let's assume it's exclusive or primary.
                // Let's return error if external was attempted but failed/invalid.
                 return new AuthResult { Success = false, Error = "Invalid credentials (External)" };
            }

            // Local Auth Flow
            var localUser = await _userManager.FindByNameAsync(username);
            if (localUser == null)
                return new AuthResult { Success = false, Error = "Invalid credentials" };
            var result = await _signInManager.CheckPasswordSignInAsync(localUser, password, false);
            if (!result.Succeeded)
                return new AuthResult { Success = false, Error = "Invalid credentials" };
            
            return await GenerateJwtTokenAsync(localUser);
        }

        private async Task<AuthResult> GenerateJwtTokenAsync(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var keyStr = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyStr) || keyStr.Length < 16)
                throw new InvalidOperationException("JWT Key is missing or too short.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );
            return new AuthResult { Success = true, Token = new JwtSecurityTokenHandler().WriteToken(token) };
        }

        public async Task<Result> RegisterAsync(string username, string password, string email, string role, CancellationToken cancellationToken = default)
        {
            var user = new IdentityUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return new Result { Success = false, Error = string.Join("; ", result.Errors.Select(e => e.Description)) };
            if (!string.IsNullOrWhiteSpace(role))
                await _userManager.AddToRoleAsync(user, role);
            return new Result { Success = true };
        }

        public async Task<Result> ChangePasswordAsync(string username, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return new Result { Success = false, Error = "User not found" };
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return new Result { Success = false, Error = string.Join("; ", result.Errors.Select(e => e.Description)) };
            return new Result { Success = true };
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            await _signInManager.SignOutAsync();
        }
    }
}
