using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class BudgetConsumptionConfiguration : IEntityTypeConfiguration<BudgetConsumption>
    {
        public void Configure(EntityTypeBuilder<BudgetConsumption> builder)
        {
            builder.HasKey(bc => bc.Id);

            builder.HasOne(bc => bc.Budget)
                .WithMany()
                .HasForeignKey(bc => bc.BudgetId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(bc => bc.Report)
                .WithMany()
                .HasForeignKey(bc => bc.ReportId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(bc => bc.Amount)
                .HasColumnType("decimal(18, 2)");

            builder.Property(bc => bc.ConsumptionDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(bc => new { bc.BudgetId, bc.ReportId })
                .IsUnique();
        }
    }
}