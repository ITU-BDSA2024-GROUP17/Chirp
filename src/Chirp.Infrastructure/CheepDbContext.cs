using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure;

public class CheepDbContext(DbContextOptions<CheepDbContext> options) : DbContext(options)
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
}
