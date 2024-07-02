using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechsysLog.Core.Identity;
using TechsysLog.Identity.Api.Data;

namespace TechsysLog.Identity.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<TechsysLogDBIdentityContext>(optionsAction: options
                  => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TechsysLogDBIdentityContext>()
                .AddDefaultTokenProviders();
            services.AddJWTConfiguration(configuration);
            return services;
        }
    }
}
