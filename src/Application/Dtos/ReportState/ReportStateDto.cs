using Simpl.Expenses.Domain.Enums;
using System;
using System.Text.Json.Serialization;

namespace Simpl.Expenses.Application.Dtos.ReportState
{
    public class ReportStateDto
    {
        public int ReportId { get; set; }
        public int WorkflowId { get; set; }
        public int CurrentStepId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReportStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}