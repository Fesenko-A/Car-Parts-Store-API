using Auth;
using DAL.Repository.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository {
    public class ApplicationDbContext : IdentityDbContext<AppUser> {

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<SpecialTag> SpecialTag { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<OnlinePayment> OnlinePayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(Connections.LOCAL);
        }
        /*
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(
                new Category {
                    Id = 1,
                    Name = "Oil"
                },
                new Category {
                    Id = 2,
                    Name = "Batteries"
                }
                );

            builder.Entity<Brand>().HasData(
                new Brand {
                    Id = 1,
                    Name = "Bosch"
                },
                new Brand {
                    Id = 2,
                    Name = "Castrol"
                }
                );

            builder.Entity<SpecialTag>().HasData(
                new SpecialTag {
                    Id = 1,
                    Name = ""
                },
                new SpecialTag {
                    Id = 2,
                    Name = "Best Seller"
                }
                );

            builder.Entity<Product>().HasData(
                new Product {
                    Id = 1,
                    BrandId = 2,
                    Name = "Castrol Magnatec Engine Oil - 10W-40 - 4ltr",
                    Description = "Non-stop protection from every start.",
                    SpecialTagId = 2,
                    CategoryId = 1,
                    Price = 34.74,
                    ImageUrl = "https://images.lteplatform.com/images/products/600x600/521771951.jpg"
                },
                new Product {
                    Id = 2,
                    BrandId = 1,
                    Name = "Bosch Car Battery 075",
                    Description = "Bosch S4 car batteries are a high quality, premium replacement for you original car battery.",
                    SpecialTagId = 1,
                    CategoryId = 2,
                    Price = 85.89,
                    ImageUrl = "https://images.lteplatform.com/images/products/600x600/444770757.jpg"
                }
                );

            builder.Entity<PaymentMethod>().HasData(
                new PaymentMethod {
                    PaymentMethodId = 1,
                    Description = "Cash"
                },
                new PaymentMethod {
                    PaymentMethodId = 2,
                    Description = "Pay online"
                }
                );
        }
        */
    }
}
