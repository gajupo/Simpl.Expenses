
namespace Simpl.Expenses.Domain.Entities
{
    public class ReimbursementDetail
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
    }
}
