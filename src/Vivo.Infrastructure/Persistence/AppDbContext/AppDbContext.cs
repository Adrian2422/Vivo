namespace Vivo.Infrastructure.Persistence.AppDbContext;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;


public class ApplicationDbContext : DbContext
{

    public DbSet<ShortenedUrlEntity> ShortenedUrls => Set<ShortenedUrlEntity>();


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrlEntity>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Code).HasMaxLength(12).IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
            entity.Property(x => x.OriginalUrl).HasMaxLength(2048).IsRequired();
        });
    }
}