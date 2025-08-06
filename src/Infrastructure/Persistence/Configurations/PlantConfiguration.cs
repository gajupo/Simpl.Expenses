
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        }
    }
}
