using Products.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Products.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.Description)
                .HasMaxLength(200);

            builder
                .HasOne(m => m.Account)
                .WithMany(a => a.Products)
                .HasForeignKey(m => m.AccountId);

            builder
                .Property(m => m.DateAdded)
                .HasDefaultValueSql("GETDATE()");

            builder
                .ToTable("Products");
        }
    }
}