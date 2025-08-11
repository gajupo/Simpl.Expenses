
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class PurchaseOrderDetailConfiguration : IEntityTypeConfiguration<PurchaseOrderDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderDetail> builder)
        {
            builder.HasKey(pod => pod.ReportId);
            builder.HasOne(pod => pod.Report).WithOne().HasForeignKey<PurchaseOrderDetail>(pod => pod.ReportId).OnDelete(DeleteBehavior.NoAction);
            
            
            builder.HasOne(pod => pod.UsoCfdi).WithMany().HasForeignKey(pod => pod.UsoCfdiId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(pod => pod.Incoterm).WithMany().HasForeignKey(pod => pod.IncotermId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
