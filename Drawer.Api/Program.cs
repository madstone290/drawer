using Drawer.Api.ActionFilters;
using Drawer.Application;
using Drawer.Infrastructure;
using Drawer.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Secrets/drawer_identity_db_secret.json");
builder.Configuration.AddJsonFile("Secrets/email_secret.json");
builder.Configuration.AddJsonFile("Secrets/jwt_settings_secret.json");

builder.Services.AddApplicationDependency();
builder.Services.AddInfrastructureDependency(builder.Configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // 기본 인증시도 스킴이 없을경우 401이후 리디렉트로 404예외가 발생한다.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? throw new Exception("JWT설정이 없습니다");
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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<DefaultExceptionFilter>();
builder.Services.AddSingleton<ExceptionCodeProvider>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();