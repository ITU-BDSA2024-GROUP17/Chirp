using Microsoft.EntityFrameworkCore;
using Chirp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class CheepDbContext : IdentityDbContext
{
    public CheepDbContext()
    {

    }

    public CheepDbContext(DbContextOptions<CheepDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        optionsBuilder.UseSqlite("Data Source=SQLiteDB.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>()
            .HasMany(a => a.Cheeps)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AuthorId);

        modelBuilder.Entity<Author>()
            .HasMany(a => a.LikedCheeps)
            .WithMany(c => c.Likes)
            .UsingEntity<CheepLike>(cl => cl.Property(cl => cl.TimeStamp).HasDefaultValueSql("CURRENT_TIMESTAMP"));
    }

    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
}
