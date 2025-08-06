
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class ReimbursementDetailConfiguration : IEntityTypeConfiguration<ReimbursementDetail>
    {
        public void Configure(EntityTypeBuilder<ReimbursementDetail> builder)
        {
            builder.HasKey(rd => rd.ReportId);
            builder.HasOne(rd => rd.Report).WithOne().HasForeignKey<ReimbursementDetail>(rd => rd.ReportId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(rd => rd.EmployeeName).IsRequired().HasMaxLength(255);
            builder.Property(rd => rd.EmployeeNumber).HasMaxLength(50);
        }
    }
}
