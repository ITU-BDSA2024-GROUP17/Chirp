using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Chirp.Infrastructure;

namespace Chirp.Web.Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IDisposable where TStartup : class
    {
        private IServiceScope? _scope;
        private CheepDbContext? _context;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove old DbContext
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CheepDbContext>));
                if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(CheepDbContext));
                if (dbConnectionDescriptor != null) services.Remove(dbConnectionDescriptor);

                // Add new in-memory test database
                services.AddDbContext<CheepDbContext>(options => options.UseInMemoryDatabase(databaseName: "WebTestDb"));
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
            // DbInitializer.clearDatabase(context);
            DbInitializer.SeedDatabase(context);
        }

        public void Dispose()
        {
            _context?.Dispose();
            _scope?.Dispose();
            base.Dispose();
        }
    }
}
