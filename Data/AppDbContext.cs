using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Product_Price", "[Price] >= 0");
                table.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0");
            });
            entity.HasKey(p => p.ProductCode);
            entity.Property(p => p.ProductCode)
                .HasMaxLength(30)
                .IsRequired()
                .ValueGeneratedNever();
            
            entity.Property(p => p.Name)
                .HasMaxLength(120)
                .IsRequired();
            entity.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(p => p.Stock)
                .IsRequired();
            entity.Property(p => p.Category)
                .HasMaxLength(30)
                .IsRequired();
        });
    }
}