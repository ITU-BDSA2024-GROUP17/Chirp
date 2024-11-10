using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Chirp.Infrastructure;

namespace Chirp.Web.Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IDisposable where TStartup : class
    {
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
        public static void TestSeedDatabase(CustomWebApplicationFactory<Program> factory)
        {
            using var scope = factory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetService<CheepDbContext>() ?? throw new Exception("TestCheepDbContext not found!");
            DetachAllEntities(context);

            DbInitializer.SeedDatabase(context);

        }

        public static void DetachAllEntities(CheepDbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
