using Kids_Toys.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kids_Toys.Data
{
    public class ApplicationDbContext : IdentityDbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AspNetUserLogin>().HasKey(l => new { l.LoginProvider, l.ProviderKey });

            builder.Entity<AspNetUserToken>().HasNoKey();

            builder.Entity<AspNetUser>().Ignore(u => u.AspNetUserTokens);


            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );

        }

        public virtual DbSet<Category> categories { get; set; }
        public virtual DbSet<Toy> toys { get; set; }

        public virtual DbSet<Cart> carts { get; set; }

        public virtual DbSet<CartDetail> cart_Details { get; set; }

        public virtual DbSet<Order> orders { set; get; }

        public virtual DbSet<OrderDetail> order_Details { set; get; }

        public virtual DbSet<Comment> comments { set; get; }

        public virtual DbSet<Discount> discounts { set; get; }


    }
}