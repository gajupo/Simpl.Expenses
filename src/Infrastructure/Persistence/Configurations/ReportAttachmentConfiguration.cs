
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class ReportAttachmentConfiguration : IEntityTypeConfiguration<ReportAttachment>
    {
        public void Configure(EntityTypeBuilder<ReportAttachment> builder)
        {
            builder.HasKey(ra => ra.Id);
            builder.HasOne(ra => ra.Report).WithMany().HasForeignKey(ra => ra.ReportId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(ra => ra.UploadedByUser).WithMany().HasForeignKey(ra => ra.UploadedByUserId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(ra => ra.FileName).IsRequired().HasMaxLength(255);
            builder.Property(ra => ra.FilePath).IsRequired().HasMaxLength(500);
            builder.Property(ra => ra.MimeType).HasMaxLength(100);
            builder.Property(ra => ra.UploadedAt).HasDefaultValueSql("GETDATE()");
        }
    }
}
