using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
var connStr = builder.Configuration.GetConnectionString("CadenaSQL");
Console.WriteLine($"Connection string: {connStr}");

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services
    .AddDbContext<LBW.Models.Entity.LbwContext>(options =>
    { object value = options.UseSqlServer(connStr); })
    .AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => {
        option.LoginPath = "/Acceso/Login";
        option.LogoutPath = "/Acceso/Logout";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(40);
        option.AccessDeniedPath = "/Acceso/Privacy";
    });

builder.Services.AddSession();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}


app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An unhandled exception has occurred.");

        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }
    }
});

app.UseSession();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();
