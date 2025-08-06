
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class UsoCFDIConfiguration : IEntityTypeConfiguration<UsoCFDI>
    {
        public void Configure(EntityTypeBuilder<UsoCFDI> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Clave).IsRequired().HasMaxLength(10);
            builder.Property(u => u.Description).IsRequired().HasMaxLength(255);
        }
    }
}
