using Core.Interfaces;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Tests;

public class CheepServiceTests
{
    private readonly CheepDbContext _cheepDbContext;
    private readonly IAuthorRepository _authorRepository;
    private readonly ICheepRepository _cheepRepository;
    private readonly CheepService _cheepService;

    public CheepServiceTests()
    {
        var options = new DbContextOptionsBuilder<CheepDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _cheepDbContext = new CheepDbContext(options);

        _authorRepository = new AuthorRepository(_cheepDbContext);
        _cheepRepository = new CheepRepository(_cheepDbContext);

        _cheepService = new CheepService(_cheepRepository, _authorRepository);
    }


}
