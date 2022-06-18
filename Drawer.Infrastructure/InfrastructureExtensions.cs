using Drawer.Application.Services.Authentication;
using Drawer.Application.Services.Authentication.Repos;
using Drawer.Domain.Models.Authentication;
using Drawer.Infrastructure.Authentication;
using Drawer.Infrastructure.Authentication.Repos;
using Drawer.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
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

                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<DrawerIdentityDbContext>()
                .AddDefaultTokenProviders();

            // 리파지토리 추가
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            var mailKitOptions = configuration.GetSection("Email").Get<MailKitOptions>();
            if (mailKitOptions == null)
                throw new Exception("이메일 설정이 없습니다");
            services.AddSingleton(mailKitOptions);
            services.AddScoped<IEmailSender, EmailSender>();

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSettings == null)
                throw new Exception("JWT 설정이 없습니다"); 
            services.AddSingleton(jwtSettings);
            services.AddScoped<ITokenGenerator, TokenGenerator>();
        }
    }
}

