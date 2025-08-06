
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class ReportTypeConfiguration : IEntityTypeConfiguration<ReportType>
    {
        public void Configure(EntityTypeBuilder<ReportType> builder)
        {
            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(rt => rt.Name).IsUnique();
        }
    }
}
