
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.ReportNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(r => r.ReportNumber).IsUnique();
            builder.Property(r => r.Name).IsRequired().HasMaxLength(255);
            builder.Property(r => r.Amount).HasColumnType("decimal(18, 2)");
            builder.Property(r => r.Currency).IsRequired().HasMaxLength(3).HasDefaultValue("MXN");
            builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.ReportType).WithMany().HasForeignKey(r => r.ReportTypeId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.Plant).WithMany().HasForeignKey(r => r.PlantId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.Category).WithMany().HasForeignKey(r => r.CategoryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.Supplier).WithMany().HasForeignKey(r => r.SupplierId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.CostCenter).WithMany().HasForeignKey(r => r.CostCenterId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.AccountProject).WithMany().HasForeignKey(r => r.AccountProjectId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.PurchaseOrderDetail)
                .WithOne(d => d.Report)
                .HasForeignKey<PurchaseOrderDetail>(d => d.ReportId);

            builder.HasOne(r => r.ReimbursementDetail)
                .WithOne(d => d.Report)
                .HasForeignKey<ReimbursementDetail>(d => d.ReportId);

            builder.HasOne(r => r.AdvancePaymentDetail)
                .WithOne(d => d.Report)
                .HasForeignKey<AdvancePaymentDetail>(d => d.ReportId);

            builder.HasOne(r => r.ReportState)
                .WithOne(rs => rs.Report)
                .HasForeignKey<ReportState>(rs => rs.ReportId);

            builder.Property(r => r.BankName).HasMaxLength(100);
            builder.Property(r => r.AccountNumber).HasMaxLength(50);
            builder.Property(r => r.Clabe).HasMaxLength(18);
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETDATE()");
        }
    }
}
