using System.Reflection;
using CSVDBService.Records;
using Microsoft.OpenApi.Models;
using SimpleDB;

var db = CSVDatabase<Cheep>.Instance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
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

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles(); // For the css file
    // app.UseHttpsRedirection(); // use in production
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.InjectStylesheet("./swaggerUI.css");
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chirp API");
        options.RoutePrefix = string.Empty; // At the root (remove later and use another endpoint)
    });
}

app.MapPost("/cheep", (Cheep cheep) =>
{
    db.Store(cheep);

    return cheep;
}).WithSummary("Sends a cheep");

app.MapGet("/cheeps", () => db.Read().ToList()).WithSummary("Retrieves the cheeps");

app.Run();
