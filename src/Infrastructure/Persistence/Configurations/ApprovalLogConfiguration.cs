
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class ApprovalLogConfiguration : IEntityTypeConfiguration<ApprovalLog>
    {
        public void Configure(EntityTypeBuilder<ApprovalLog> builder)
        {
            builder.HasKey(al => al.Id);
            builder.HasOne(al => al.Report).WithMany().HasForeignKey(al => al.ReportId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(al => al.User).WithMany().HasForeignKey(al => al.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(al => al.Action).IsRequired().HasMaxLength(20);
            builder.Property(al => al.LogDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
