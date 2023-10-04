using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using shopping_list.Models.Entities;
using shopping_list.Models.Identity;

namespace shopping_list.Data
{
    public class MyContext : IdentityDbContext<ApplicationUser>
    {
        public MyContext(DbContextOptions options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasIndex(x => x.Id);
                entity.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
            });

            builder.Entity<Category>(entity =>
            {
                entity.HasIndex(x => x.Id);
            });

            builder.Entity<ShoppingList>(entity =>
            {
                entity.HasIndex(x => x.Id);
            });

            builder.Entity<ProductShoppingList>(entity =>
            {
                entity.HasKey(x => new { x.ProductId, x.ShoppingListId });
                entity.HasOne(x => x.Product).WithMany(x => x.ProductShoppingLists).HasForeignKey(x => x.ProductId);
                entity.HasOne(x => x.ShoppingList).WithMany(x => x.ProductShoppingLists).HasForeignKey(x => x.ShoppingListId);
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.HasData(new IdentityRole
                {
                    Id= "1",
                    Name= "Admin",
                    NormalizedName= "ADMIN"
                });
                entity.HasData(new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                });
            });
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ShoppingList> ShoppingList { get; set; }
        public DbSet<ProductShoppingList> ProductShoppingList { get; set; }

    };
}
