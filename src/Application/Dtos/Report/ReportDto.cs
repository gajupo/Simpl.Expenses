using System;

namespace Simpl.Expenses.Application.Dtos.Report
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string ReportNumber { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string ReportDescription { get; set; }
        public DateTime ReportDate { get; set; }
        public int UserId { get; set; }
        public int ReportTypeId { get; set; }
        public string ReportTypeName { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Clabe { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public int? AccountProjectId { get; set; }
        public string AccountProjectName { get; set; }

        public PurchaseOrderDetailDto PurchaseOrderDetail { get; set; }
        public AdvancePaymentDetailDto AdvancePaymentDetail { get; set; }
        public ReimbursementDetailDto ReimbursementDetail { get; set; }
    }
}
