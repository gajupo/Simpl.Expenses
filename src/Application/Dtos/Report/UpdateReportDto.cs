namespace Simpl.Expenses.Application.Dtos.Report
{
    public class UpdateReportDto
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int UserId { get; set; }
        public int ReportTypeId { get; set; }
        public int PlantId { get; set; }
        public int CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Clabe { get; set; }
        public int CostCenterId { get; set; }
        public int? AccountProjectId { get; set; }

        public UpdatePurchaseOrderDetailDto? PurchaseOrderDetail { get; set; }
        public UpdateAdvancePaymentDetailDto? AdvancePaymentDetail { get; set; }
        public UpdateReimbursementDetailDto? ReimbursementDetail { get; set; }
    }
}
