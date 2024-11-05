// test/Chirp.Web.Tests/CustomWebApplicationFactory.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Chirp.Core.Interfaces;
using Chirp.Infrastructure;
using Moq;

namespace Chirp.Web.Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public Mock<ICheepRepository> MockCheepRepository { get; } = new Mock<ICheepRepository>();
        public Mock<IAuthorRepository> MockAuthorRepository { get; } = new Mock<IAuthorRepository>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(CheepDbContext));

                services.AddDbContext<CheepDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryCheepDb");
                });

                services.RemoveAll<ICheepRepository>();
                services.AddSingleton(MockCheepRepository.Object);

                services.RemoveAll<IAuthorRepository>();
                services.AddSingleton(MockAuthorRepository.Object);
            });
        }
    }
}
