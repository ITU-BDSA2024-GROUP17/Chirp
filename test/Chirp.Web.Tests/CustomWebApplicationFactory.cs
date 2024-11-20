using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Chirp.Infrastructure;

namespace Chirp.Web.Tests.Integration;

/// <summary>
/// Custom Web App factory used for tests.
/// Removes old type of database context and created in memory database.
/// </summary>
/// <typeparam name="TStartup">Class to factor application</typeparam>
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IDisposable where TStartup : class
{
    /// <summary>
    /// Configure
    /// </summary>
    /// <param name="builder">WebHostBuilder</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove old DbContext
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CheepDbContext>));
            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(CheepDbContext));
            if (dbConnectionDescriptor != null) services.Remove(dbConnectionDescriptor);

            // Add nex in Memory test database
            services.AddDbContext<CheepDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "WebTestDb");
            });
        });
    }

    /// <summary>
    /// Seed the test database
    /// </summary>
    /// <param name="factory"></param>
    /// <exception cref="Exception"></exception>
    public static CheepDbContext TestSeedDatabase(CustomWebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<CheepDbContext>() ?? throw new Exception("TestCheepDbContext not found!");
        DetachAllEntities(context);
        WebDbInitializer.SeedDatabase(context);
        return context;
    }

    /// <summary>
    /// Detach all entities from being tracked in the database
    /// </summary>
    /// <param name="context">Database context to effect</param>
    public static void DetachAllEntities(CheepDbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries().ToList())
        {
            entry.State = EntityState.Detached;
        }
    }
    /// <summary>
    /// Dispose base database context.
    /// </summary>
    public new void Dispose()
    {
        base.Dispose();
    }
}
