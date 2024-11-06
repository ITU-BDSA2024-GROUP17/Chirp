// test/Chirp.Web.Tests/CustomWebApplicationFactory.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Chirp.Core.Interfaces;
using Moq;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repositories;
using Chirp.Core.Entities;

namespace Chirp.Web.Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove old DbContext
                var dbContextDescriptor = services.SingleOrDefault(
               d => d.ServiceType ==
                   typeof(DbContextOptions<CheepDbContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(CheepDbContext));

                services.Remove(dbConnectionDescriptor);

                services.AddDbContext<CheepDbContext>(options => options.UseInMemoryDatabase(databaseName: "WebTestDb"));
            });
        }
        public static async Task TestSeedDatabase(CustomWebApplicationFactory<Program> factory)
        {
            using var scope = factory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetService<CheepDbContext>() ?? throw new Exception("TestCheepDbContext not found!");
            DbInitializer.SeedDatabase(context);
        }
    }
}
