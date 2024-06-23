using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Behaviours;
using MediatR;

namespace Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => {

                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            });

            return services;
        }
    }
}