﻿using Drawer.Application.Services;
using Drawer.Application.Services.Authentication;
using Drawer.Application.Services.Authentication.Repos;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Application.Services.Organization;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Infrastructure.Data;
using Drawer.Infrastructure.Repos;
using Drawer.Infrastructure.Repos.Authentication;
using Drawer.Infrastructure.Repos.Inventory;
using Drawer.Infrastructure.Repos.Organization;
using Drawer.Infrastructure.Repos.UserInformation;
using Drawer.Infrastructure.Services.Authentication;
using Drawer.Infrastructure.Services.Organization;
using Drawer.Infrastructure.Services.UserInformation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            var connectionString = configuration["DrawerDb:ConnectionString"];
            services.AddDbContext<DrawerDbContext>(options =>
            {
                options.UseNpgsql(connectionString).EnableSensitiveDataLogging();
            });

            // AspNetCore Identity 기본설정
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<DrawerDbContext>()
                .AddDefaultTokenProviders();

          
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
                    ?? throw new Exception("JWT설정이 없습니다");

            // jwt인증
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // 기본 인증시도 스킴이 없을경우 401이후 리디렉트로 404예외가 발생한다.
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = (c) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = (c) =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

            var mailKitOptions = configuration.GetSection("Email").Get<MailKitOptions>();
            if (mailKitOptions == null)
                throw new Exception("이메일 설정이 없습니다");
            services.AddSingleton(mailKitOptions);
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddSingleton(jwtSettings);

            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.AddScoped<ICompanyIdProvider, CompanyIdProvider>();
            services.AddScoped<IUserIdProvider, UserIdProvider>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyMemberRepository, CompanyMemberRepository>();
            services.AddScoped<ICompanyJoinRequestRepository, CompanyJoinRequestRepository>();

            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ILocationGroupRepository, LocationGroupRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
            services.AddScoped<IReceiptRepository, ReceiptRepository>();
            services.AddScoped<IIssueRepository, IssueRepository>();
            services.AddScoped<ILayoutRepository, LayoutRepository>();

        }
    }
}

