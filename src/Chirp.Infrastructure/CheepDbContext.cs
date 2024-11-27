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

        modelBuilder.Entity<Author>()
            .HasMany(a => a.Following)
            .WithMany(a => a.Followers)
            .UsingEntity<AuthorFollow>(
                join => join
                    .HasOne(af => af.Followee)
                    .WithMany()
                    .HasForeignKey("FolloweeId"),
                join => join
                    .HasOne(af => af.Follower)
                    .WithMany()
                    .HasForeignKey("FollowerId"),
                join =>
                {
                    join.HasKey("FollowerId", "FolloweeId");
                    join.Property(af => af.TimeStamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
                }
            );

        modelBuilder.Entity<Cheep>()
            .HasMany(c => c.Comments)
            .WithOne(c => c.CheepOwner)
            .HasForeignKey(c => c.CheepOwnerId);
    }

    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
}
