using Microsoft.EntityFrameworkCore;

namespace fitnessapi.Models;

public class FitnessContext : DbContext
{
    public FitnessContext(DbContextOptions<FitnessContext> options)
    : base(options)
    {
    }

    public DbSet<Badge> Badges { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Vote> Votes { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<SearchResult> SearchResults { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new SearchResultConfiguration());
    }
}


//public DbSet<PostHistoryItem> PostHistory { get; set; } = null!;
//public DbSet<PostLink> PostLinks { get; set; } = null!;
//public DbSet<Tag> Tags { get; set; } = null!;

