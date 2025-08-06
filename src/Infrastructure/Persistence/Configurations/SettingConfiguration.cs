
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.HasKey(s => s.Key);
            builder.Property(s => s.Key).HasMaxLength(50);
            builder.Property(s => s.Value).IsRequired().HasMaxLength(255);
        }
    }
}
