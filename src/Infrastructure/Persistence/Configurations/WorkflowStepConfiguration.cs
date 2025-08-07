
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
    {
        public void Configure(EntityTypeBuilder<WorkflowStep> builder)
        {
            builder.HasKey(ws => ws.Id);
            builder.Property(ws => ws.Name).IsRequired().HasMaxLength(100);
            builder.HasOne(ws => ws.Workflow).WithMany(w => w.Steps).HasForeignKey(ws => ws.WorkflowId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ws => ws.ApproverRole).WithMany().HasForeignKey(ws => ws.ApproverRoleId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
