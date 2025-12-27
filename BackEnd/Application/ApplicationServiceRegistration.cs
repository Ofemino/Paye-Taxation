using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Paye.Application.Behaviors;
using System.Reflection;

namespace Paye.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Register AutoMapper if you add it later
            // services.AddAutoMapper(assembly);

            // Register MediatR
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // Register Validators
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
