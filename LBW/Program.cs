using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("CadenaSQL");

// Configurar los límites de solicitud
const int maxRequestSize = 209715200; // 200 MB en bytes

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = maxRequestSize;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = maxRequestSize;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = maxRequestSize;
    options.MultipartBodyLengthLimit = maxRequestSize;
    options.MultipartHeadersLengthLimit = maxRequestSize;
});

// Configurar el límite de tamaño de archivo para IIS Express
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = maxRequestSize;
});

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Configurar el límite de tamaño de archivo para Web API si lo estás usando
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.ValueLengthLimit = maxRequestSize;
    options.MultipartBodyLengthLimit = maxRequestSize;
    options.MultipartHeadersLengthLimit = maxRequestSize;
});

// Resto de su configuración...
builder.Services
    .AddDbContext<LBW.Models.Entity.LbwContext>(options =>
    { options.UseSqlServer(connStr); })
    .AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.LogoutPath = "/Acceso/Logout";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();
