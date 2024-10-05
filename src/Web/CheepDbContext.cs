using Microsoft.EntityFrameworkCore;
using Web.Entities;

namespace Web;

public class CheepDbContext(DbContextOptions<CheepDbContext> options) : DbContext(options)
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
}
