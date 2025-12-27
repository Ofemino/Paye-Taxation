using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paye.Infrastructure.Persistence;
using Serilog;
using Paye.Infrastructure.BackgroundJobs;
using MediatR;
using Paye.Application.Contracts;
using Paye.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Paye.Infrastructure.Configuration;

using Paye.Infrastructure.Persistence.Interceptors;

namespace Paye.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind Settings
            services.Configure<AuthenticationSettings>(configuration.GetSection("AuthenticationSettings"));
            services.Configure<ExternalAuthSettings>(configuration.GetSection("ExternalAuthSettings"));

            // Interceptors
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            // Database
            services.AddDbContext<PayeDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                       .AddInterceptors(interceptor);
            });

            // Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<PayeDbContext>()
                .AddDefaultTokenProviders();

            // HTTP Client for External Auth
            services.AddHttpClient<IExternalAuthProvider, ExternalAuthProvider>();

            // Authentication Configuration (JWT)
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            // Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            // File Storage
            services.AddSingleton<IFileStorageService, LocalFileStorageService>();

            // Background jobs
            services.AddHostedService<NotificationBackgroundService>();

            // Register MediatR handlers from Infrastructure
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Paye.Infrastructure.Features.TaxRelief.Commands.CreateTaxReliefSubmissionCommandHandler).Assembly);
            });

            // Register AuthService
            services.AddScoped<IAuthService, AuthService>();

            // TODO: Add audit logging, virus scanning, etc.

            return services;
        }
    }

    // Stub for secure file storage
    public interface IFileStorageService
    {
        Task<string> SaveAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    }

    public class FileStorageServiceStub : IFileStorageService
    {
        public Task<string> SaveAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            // Simulate secure, randomized, non-public storage
            return Task.FromResult($"/files/{Guid.NewGuid()}_{fileName}");
        }
    }
}