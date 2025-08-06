
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Amount).HasColumnType("decimal(18, 2)");
            builder.HasOne(b => b.CostCenter).WithMany().HasForeignKey(b => b.CostCenterId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(b => b.AccountProject).WithMany().HasForeignKey(b => b.AccountProjectId).OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(b => new { b.CostCenterId, b.AccountProjectId, b.StartDate, b.EndDate }).IsUnique();
        }
    }
}
