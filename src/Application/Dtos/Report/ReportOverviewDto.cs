using System;
using Simpl.Expenses.Domain.Enums;

namespace Simpl.Expenses.Application.Dtos.Report
{
    public class ReportOverviewDto
    {
        public int Id { get; set; }
        public string ReportNumber { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int UserId { get; set; }
        public int ReportTypeId { get; set; }
        public string ReportTypeName { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? AccountProjectId { get; set; }
        public string AccountProjectName { get; set; }
        public string? Status { get; set; }
        public int? CurrentStepId { get; set; }
        public string CurrentStepName { get; set; }
    }
}
