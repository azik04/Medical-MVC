using MedicalMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalMVC.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ProductPhoto>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Products).WithMany(x => x.Photos).HasForeignKey(x => x.ProductId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasData(
                new User
                {
                    Id = 1,
                    UserName = "Nurida",
                    UserPassword = "Nurida2002!"
                }
            );
        });
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductPhoto> Photos { get; set; }
    public DbSet<User> Users { get; set; }
}