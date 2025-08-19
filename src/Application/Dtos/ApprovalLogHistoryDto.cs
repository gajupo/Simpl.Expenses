namespace Simpl.Expenses.Application.Dtos
{
    public class ApprovalLogHistoryDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ApprovalActionName { get; set; }
        public string Comment { get; set; }
        public string LogDate { get; set; }
    }
}
