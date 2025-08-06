
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class AccountProjectConfiguration : IEntityTypeConfiguration<AccountProject>
    {
        public void Configure(EntityTypeBuilder<AccountProject> builder)
        {
            builder.HasKey(ap => ap.Id);
            builder.Property(ap => ap.Name).IsRequired().HasMaxLength(255);
            builder.Property(ap => ap.Code).HasMaxLength(50);
        }
    }
}
