
namespace Simpl.Expenses.Domain.Entities
{
    public class AdvancePaymentDetail
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public string OrderNumber { get; set; }
    }
}
