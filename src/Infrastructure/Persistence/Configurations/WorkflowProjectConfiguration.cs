
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class WorkflowProjectConfiguration : IEntityTypeConfiguration<WorkflowProject>
    {
        public void Configure(EntityTypeBuilder<WorkflowProject> builder)
        {
            builder.HasKey(wp => new { wp.WorkflowId, wp.ProjectId });
            builder.HasOne(wp => wp.Workflow).WithMany().HasForeignKey(wp => wp.WorkflowId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(wp => wp.Project).WithMany().HasForeignKey(wp => wp.ProjectId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
