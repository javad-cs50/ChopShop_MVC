using ChopShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ChopShop.DataAccess.Data;

public class ApplicationDbContext: IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { DisplayOrder = 1,Name="T-Shirt",Id=1 },
            new Category { DisplayOrder = 2, Name = "Shirt", Id = 2 },
            new Category { DisplayOrder = 3,Name="Coat",Id=3 }
            );
        modelBuilder.Entity<Product>().HasData(
            new Product {Id=1,Title="Star T-Shirt",Description="Black StarLike T-Shirt",Price=25,CategoryId=1 },
            new Product {Id=2,Title="Fantasy T-Shirt",Description="A Custom T-Shirt",Price=30,CategoryId=1 }
            

            );
    }
}
