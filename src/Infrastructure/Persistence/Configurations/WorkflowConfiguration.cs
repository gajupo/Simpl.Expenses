
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
    {
        public void Configure(EntityTypeBuilder<Workflow> builder)
        {
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(w => w.Name).IsUnique();
            builder.Property(w => w.Description).HasMaxLength(255);
        }
    }
}
