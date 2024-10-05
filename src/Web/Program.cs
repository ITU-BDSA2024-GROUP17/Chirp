using System.Reflection;
using Microsoft.OpenApi.Models;
using SimpleDB;
using SimpleDB.Records;
using Web.Services;
using Web.Interfaces;
using Microsoft.EntityFrameworkCore;
using Web;

var db = SQLiteDatabase.Instance;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CheepDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepService, CheepService>();

builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = Environment.GetEnvironmentVariable("CHIRP_VERSION") ?? "Development",
        Title = "Chirp API",
        Description = "The API used to interact with the Chirp platform.",
        Contact = new OpenApiContact
        {
            Name = "Github",
            Url = new Uri("https://github.com/ITU-BDSA2024-GROUP17/Chirp")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://github.com/ITU-BDSA2024-GROUP17/Chirp/blob/main/LICENSE")
        }

    });
});

var app = builder.Build();

// Create a disposable service scope
using (var scope = app.Services.CreateScope())
{
    // From the scope, get an instance of our database context.
    // Through the `using` keyword, we make sure to dispose it after we are done.
    using var context = scope.ServiceProvider.GetService<CheepDbContext>();

    // Execute the migration from code.
    context?.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chirp API");
    });
}

app.MapPost("/cheep", (Cheep cheep) =>
{
    db.Store(cheep);

    return cheep;
}).WithSummary("Sends a cheep");

app.MapGet("/cheeps", () => db.Read().Result).WithSummary("Retrieves the cheeps");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
