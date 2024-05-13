using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Database;

public class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cat> Cats { get; set; } = default!;
    public virtual DbSet<Kitten> Kittens { get; set; } = default!;
    public virtual DbSet<ManyKeyedCat> ManyKeyedCats { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>(entity =>
        {
            entity.HasKey(c => new { c.Id });
            entity.Property(c => c.Id).ValueGeneratedNever();
            entity.Property(c => c.Name).HasMaxLength(100);
            entity.Property(c => c.Eman).HasMaxLength(100);
            entity.Property(c => c.Random).HasMaxLength(100);
        });

        modelBuilder.Entity<Kitten>(entity =>
        {
            entity.HasKey(k => new { k.Id });
            entity.Property(k => k.Id).ValueGeneratedNever();
            entity.Property(k => k.Name).HasMaxLength(100);

            entity
                .HasOne(k => k.Parent)
                .WithMany(c => c.Kittens)
                .HasForeignKey(k => k.ParentId)
                .IsRequired();
        });

        modelBuilder.Entity<ManyKeyedCat>(entity =>
        {
            entity.HasKey(c => new { c.Id1, c.Id2, c.Id3 });
            entity.Property(c => c.Id1).ValueGeneratedNever();
            entity.Property(c => c.Id2).ValueGeneratedNever();
            entity.Property(c => c.Id3).ValueGeneratedNever().HasMaxLength(100);
            entity.Property(c => c.Name).HasMaxLength(100);
            entity.Property(c => c.Eman).HasMaxLength(100);
            entity.Property(c => c.Random).HasMaxLength(100);
        });
    }
}