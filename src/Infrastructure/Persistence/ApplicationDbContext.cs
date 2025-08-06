
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPlant> UserPlants { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }
        public DbSet<WorkflowProject> WorkflowProjects { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<AccountProject> AccountProjects { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<UsoCFDI> UsoCFDIs { get; set; }
        public DbSet<Incoterm> Incoterms { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportState> ReportStates { get; set; }
        public DbSet<ApprovalLog> ApprovalLogs { get; set; }
        public DbSet<ReportAttachment> ReportAttachments { get; set; }
        public DbSet<ReimbursementDetail> ReimbursementDetails { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<AdvancePaymentDetail> AdvancePaymentDetails { get; set; }
        public DbSet<BudgetConsumption> BudgetConsumptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
