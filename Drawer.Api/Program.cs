using Drawer.Api;
using Drawer.Api.ActionFilters;
using Drawer.Api.Logging;
using Drawer.Api.Swagger;
using Drawer.Application;
using Drawer.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.HandleArgs(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("Secrets/drawer_development_db_secret.json");
}
else
{
    builder.Configuration.AddJsonFile("Secrets/drawer_production_db_secret.json");
}

builder.Configuration.AddJsonFile("Secrets/email_secret.json");
builder.Configuration.AddJsonFile("Secrets/jwt_settings_secret.json");
builder.Configuration.AddJsonFile("Secrets/serilog_secret.json");

builder.AddSerilog();

builder.Services.AddApplicationDependency();
builder.Services.AddInfrastructureDependency(builder.Configuration);

builder.Services.AddControllers(options =>
{
    var noContentFormatter = options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
    if (noContentFormatter != null)
    {
        noContentFormatter.TreatNullValueAsNoContent = false;
    }
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();

builder.Services.AddScoped<DefaultExceptionFilter>();
builder.Services.AddSingleton<ExceptionCodeProvider>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();