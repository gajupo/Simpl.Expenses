
namespace Simpl.Expenses.Domain.Entities
{
    public class UserPlant
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
    }
}
