
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class AdvancePaymentDetailConfiguration : IEntityTypeConfiguration<AdvancePaymentDetail>
    {
        public void Configure(EntityTypeBuilder<AdvancePaymentDetail> builder)
        {
            builder.HasKey(apd => apd.ReportId);
            builder.HasOne(apd => apd.Report).WithOne().HasForeignKey<AdvancePaymentDetail>(apd => apd.ReportId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(apd => apd.OrderNumber).HasMaxLength(100);
        }
    }
}
