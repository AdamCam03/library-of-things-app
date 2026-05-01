using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("StarterApp.Database.appsettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            connectionString = config.GetConnectionString("DevelopmentConnection");
        }

        optionsBuilder.UseNpgsql(connectionString);
    }

    // Database tables
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.Email).HasMaxLength(255);
            entity.Property(user => user.FirstName).HasMaxLength(100);
            entity.Property(user => user.LastName).HasMaxLength(100);
            entity.Property(user => user.PasswordHash).HasMaxLength(255);
            entity.Property(user => user.PasswordSalt).HasMaxLength(255);
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(role => role.Name).IsUnique();
            entity.Property(role => role.Name).HasMaxLength(100);
            entity.Property(role => role.Description).HasMaxLength(500);
        });

        // Configure UserRole entity
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasIndex(userRole => new { userRole.UserId, userRole.RoleId }).IsUnique();

            entity.HasOne(userRole => userRole.User)
                  .WithMany(user => user.UserRoles)
                  .HasForeignKey(userRole => userRole.UserId);

            entity.HasOne(userRole => userRole.Role)
                  .WithMany(role => role.UserRoles)
                  .HasForeignKey(userRole => userRole.RoleId);
        });

        // Configure Item entity
        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(item => item.Title).HasMaxLength(200);
            entity.Property(item => item.Description).HasMaxLength(1000);
            entity.Property(item => item.Category).HasMaxLength(100);
            entity.Property(item => item.LocationName).HasMaxLength(200);
            entity.Property(item => item.DailyRate).HasColumnType("decimal(10,2)");
        });

        // Configure Rental entity
        modelBuilder.Entity<Rental>(entity =>
        {
            entity.Property(rental => rental.Status).HasMaxLength(50);
        });
    }
}