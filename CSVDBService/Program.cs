using CSVDBService.Records;
using Microsoft.OpenApi.Models;
using SimpleDB;

var db = CSVDatabase<Cheep>.Instance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = Environment.GetEnvironmentVariable("CHIRP_VERSION") ?? "Development",
        Title = "Chirp API",
        Description = "The API used to interact with the Chirp platform.",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/cheep", (Cheep cheep) =>
{
    db.Store(cheep);

    return cheep;
}).WithSummary("Posts a new cheep");

app.MapGet("/cheeps", () => db.Read().ToList())
.WithSummary("Retrieves the cheeps");

app.Run();
