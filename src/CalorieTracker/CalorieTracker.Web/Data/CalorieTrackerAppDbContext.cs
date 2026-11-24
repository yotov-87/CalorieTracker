using CalorieTracker.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalorieTracker.Web.Data;

public class CalorieTrackerAppDbContext : DbContext
{
    public CalorieTrackerAppDbContext(DbContextOptions<CalorieTrackerAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .HasMaxLength(128);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.DailyCalorieGoal)
                .HasDefaultValue(2000);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<UserEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAtUtc = utcNow;
                entry.Entity.UpdatedAtUtc = utcNow;
                entry.Entity.IsSynced = true;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAtUtc = utcNow;
                entry.Entity.IsSynced = true;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}