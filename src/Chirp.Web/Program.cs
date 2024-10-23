using System.Threading.Tasks.Dataflow;
using Chirp.Core.Interfaces;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repositories;
using Chirp.Infrastructure.Services;
using Chirp.Web.Endpoints;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// Builder setup
var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CheepDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddRazorPages();

builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<CheepService>();

builder.Services.AddAuthentication(options =>
{
    options.RequireAuthenticatedSignIn = true;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = "Github";
})
.AddCookie()
.AddGitHub(options =>
{
    options.ClientId = builder.Configuration["GHUB_CLIENT_ID"] ?? throw new Exception("GitHub ClientId must be set in the configuration!");
    options.ClientSecret = builder.Configuration["GHUB_CLIENT_SECRET"] ?? throw new Exception("GitHub ClientSecret must be set in the configuration!");

    options.ReturnUrlParameter = "/signin-github";
});

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Application setup
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetService<CheepDbContext>() ?? throw new Exception("CheepDbContext not found!");

    context.Database.Migrate();
    DbInitializer.SeedDatabase(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession(); // Use session middleware
app.MapCheepEndpoints(app.Services.CreateScope().ServiceProvider.GetService<CheepService>() ?? throw new Exception("CheepService not found!"));
app.MapRazorPages();
app.Run();
