
namespace Simpl.Expenses.Domain.Entities
{
    public class PurchaseOrderDetail
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public int CostCenterId { get; set; }
        public CostCenter CostCenter { get; set; }
        public int? AccountProjectId { get; set; }
        public AccountProject AccountProject { get; set; }
        public int UsoCfdiId { get; set; }
        public UsoCFDI UsoCfdi { get; set; }
        public int IncotermId { get; set; }
        public Incoterm Incoterm { get; set; }
    }
}
