using Microsoft.EntityFrameworkCore;
using Chirp.Core.Entities;

namespace Chirp.Infrastructure;

public class CheepDbContext(DbContextOptions<CheepDbContext> options) : DbContext(options)
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
}
