using Drawer.Api.ActionFilters;
using Drawer.Application;
using Drawer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Secrets/drawer_identity_db_secret.json");
builder.Configuration.AddJsonFile("Secrets/email_secret.json");
builder.Configuration.AddJsonFile("Secrets/jwt_settings_secret.json");

builder.Services.AddApplicationDependency();
builder.Services.AddInfrastructureDependency(builder.Configuration);

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