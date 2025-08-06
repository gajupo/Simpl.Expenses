
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class IncotermConfiguration : IEntityTypeConfiguration<Incoterm>
    {
        public void Configure(EntityTypeBuilder<Incoterm> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Clave).IsRequired().HasMaxLength(10);
            builder.Property(i => i.Description).IsRequired().HasMaxLength(255);
        }
    }
}
