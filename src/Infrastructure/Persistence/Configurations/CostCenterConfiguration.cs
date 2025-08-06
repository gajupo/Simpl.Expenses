
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class CostCenterConfiguration : IEntityTypeConfiguration<CostCenter>
    {
        public void Configure(EntityTypeBuilder<CostCenter> builder)
        {
            builder.HasKey(cc => cc.Id);
            builder.Property(cc => cc.Name).IsRequired().HasMaxLength(255);
            builder.Property(cc => cc.Code).HasMaxLength(50);
        }
    }
}
