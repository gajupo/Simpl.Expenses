
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class ReportStateConfiguration : IEntityTypeConfiguration<ReportState>
    {
        public void Configure(EntityTypeBuilder<ReportState> builder)
        {
            builder.HasKey(rs => rs.ReportId);
            builder.HasOne(rs => rs.Report).WithOne(r => r.ReportState).HasForeignKey<ReportState>(rs => rs.ReportId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(rs => rs.Workflow).WithMany().HasForeignKey(rs => rs.WorkflowId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(rs => rs.CurrentStep).WithMany().HasForeignKey(rs => rs.CurrentStepId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(rs => rs.Status).IsRequired().HasMaxLength(20);
            builder.Property(rs => rs.UpdatedAt).HasDefaultValueSql("GETDATE()");
        }
    }
}
