
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(255);
            builder.Property(s => s.Rfc).HasMaxLength(20);
            builder.HasIndex(s => s.Rfc).IsUnique();
            builder.Property(s => s.Address).HasMaxLength(255);
            builder.Property(s => s.Email).HasMaxLength(100);
        }
    }
}
