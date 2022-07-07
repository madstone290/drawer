using Drawer.Shared;
using Drawer.WebClient.Api;
using Drawer.WebClient.Authentication;
using Drawer.WebClient.Presenters;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices(options =>
{
    options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    options.SnackbarConfiguration.PreventDuplicates = false;
    options.SnackbarConfiguration.NewestOnTop = false;
    options.SnackbarConfiguration.ShowCloseIcon = true;
    options.SnackbarConfiguration.MaxDisplayedSnackbars = 10;
    options.SnackbarConfiguration.VisibleStateDuration = 5000;
    options.SnackbarConfiguration.ShowTransitionDuration = 0;
    options.SnackbarConfiguration.HideTransitionDuration = 1000;

    options.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Drawer.Blazor.Cookie";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.CompanyOwner, policy =>
        policy.RequireClaim(DrawerClaimTypes.IsCompanyOwner, "true", "True"));
    options.AddPolicy(Policies.CompanyMember, policy =>
        policy.RequireClaim(DrawerClaimTypes.IsCompanyMember, "true", "True"));
});

// Drawer.Api 클라이언트
builder.Services.AddSingleton<HttpClient>((sp) => new HttpClient()
{
    BaseAddress = new Uri("https://localhost:6001")
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITokenStorage, TokenStorage>();
builder.Services.AddScoped<ITokenManager, TokenManager>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();

builder.Services.Scan(selector =>
{
    selector.FromAssemblyOf<ApiClient>()
        .AddClasses(classes => classes.AssignableTo<ApiClient>())
        .AsSelf()
        .WithScopedLifetime();

    selector.FromAssemblyOf<IPresenter>()
        .AddClasses(classes => classes.AssignableTo<IPresenter>())
        .AsSelf()
        .WithScopedLifetime();
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
});
app.MapFallbackToPage("/_Host");

app.Run();

