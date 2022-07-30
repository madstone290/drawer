using Blazored.LocalStorage;
using Drawer.Shared;
using Drawer.Web;
using Drawer.Web.Api;
using Drawer.Web.Authentication;
using Drawer.Web.Encryption;
using Drawer.Web.Frontend;
using Drawer.Web.Presenters;
using Drawer.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.HandleArgs(args);

builder.Services.AddBlazoredLocalStorage();

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
builder.Services.AddSingleton<HttpClient>((sp) =>
{
    var apiAddress = builder.Environment.IsDevelopment()
        ? new Uri(builder.Configuration["DrawerApiLocalhost"])
        : new Uri(builder.Configuration["DrawerApiDomain"]);

    return new HttpClient() 
    {  
        BaseAddress = apiAddress 
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITokenStorage, TokenStorage>();
builder.Services.AddScoped<ITokenManager, TokenManager>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddScoped<ILocalStorage, EncryptionLocalStorage>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IExcelFileService, ExcelFileService>();
builder.Services.AddTransient<ILockService, LockService>();

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

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
});
app.MapFallbackToPage("/_Host");

app.Run();


