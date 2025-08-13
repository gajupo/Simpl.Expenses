
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence.Configurations
{
    public class UserPlantConfiguration : IEntityTypeConfiguration<UserPlant>
    {
        public void Configure(EntityTypeBuilder<UserPlant> builder)
        {
            builder.HasKey(up => new { up.UserId, up.PlantId });
            builder.HasOne(up => up.User).WithMany(u => u.UserPlants).HasForeignKey(up => up.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(up => up.Plant).WithMany().HasForeignKey(up => up.PlantId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
