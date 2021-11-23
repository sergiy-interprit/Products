using Microsoft.EntityFrameworkCore;
using Products.Domain.Models;
using Products.Data.Configurations;

namespace Products.Data
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new ProductConfiguration());

            builder
                .ApplyConfiguration(new AccountConfiguration());
        }
    }
}
