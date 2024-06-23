using Application.Common.Interfaces;
using Domain.Identity;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, string wwwrootPath)
        {

            var connectionString = configuration.GetConnectionString("MSSQLDB")!;

            // DbContext
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>(options =>
            {

                // User Password Options
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                // User Username and Email Options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+$";
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // Scoped Services
            services.AddScoped<IAuthenticationService, AuthenticationManager>();
            services.AddSingleton<IJwtService, JwtManager>();

            // Singleton Services
            services.AddSingleton<IEmailService>(new EmailManager(wwwrootPath));


            return services;
        }
    }
}