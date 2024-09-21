using CSVDBService.Records;
using SimpleDB;

var db = CSVDatabase<Cheep>.Instance;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/cheep", (Cheep cheep) =>
{
    db.Store(cheep);

    return cheep;
});

app.MapGet("/cheeps", () => db.Read().ToList());

app.Run();
