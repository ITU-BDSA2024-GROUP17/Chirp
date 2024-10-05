using Web.Services;
using Web.Interfaces;
using Microsoft.EntityFrameworkCore;
using Web;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CheepDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddRazorPages();

builder.Services.AddScoped<ICheepService, CheepService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetService<CheepDbContext>();
    context?.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
