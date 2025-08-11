
namespace Simpl.Expenses.Domain.Entities
{
    public class PurchaseOrderDetail
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }
        
        
        public int UsoCfdiId { get; set; }
        public UsoCFDI UsoCfdi { get; set; }
        public int IncotermId { get; set; }
        public Incoterm Incoterm { get; set; }
    }
}
