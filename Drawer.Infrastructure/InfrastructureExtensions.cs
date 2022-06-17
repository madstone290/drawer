using Drawer.Domain.Models.Authentication;
using Drawer.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static void AddInfrastructureDependency(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DrawerIdentityDb:ConnectionString"];
            services.AddDbContext<DrawerIdentityDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // AspNetCore Identity 기본설정
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 2;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DrawerIdentityDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
