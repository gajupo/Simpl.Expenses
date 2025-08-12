using System.Collections.Generic;

namespace Simpl.Expenses.Domain.Constants
{
    public static class PermissionCatalog
    {
        public const string ExpensesRead = "Expenses.Read";
        public const string ExpensesCreate = "Expenses.Create";
        public const string ExpensesUpdate = "Expenses.Update";
        public const string ExpensesDelete = "Expenses.Delete";
        public const string ExpensesApprove = "Expenses.Approve";
        public const string UsersManage = "Users.Manage";
        public const string RolesManage = "Roles.Manage";
        public const string PermissionsManage = "Permissions.Manage";

        // workflow permissions
        public const string WorkflowRead = "Workflow.Read";
        public const string WorkflowCreate = "Workflow.Create";
        public const string WorkflowUpdate = "Workflow.Update";
        public const string WorkflowDelete = "Workflow.Delete";

        // master data + other resources permissions
        public const string AccountProjectRead = "AccountProject.Read";
        public const string AccountProjectCreate = "AccountProject.Create";
        public const string AccountProjectUpdate = "AccountProject.Update";
        public const string AccountProjectDelete = "AccountProject.Delete";

        public const string BudgetRead = "Budget.Read";
        public const string BudgetCreate = "Budget.Create";
        public const string BudgetUpdate = "Budget.Update";
        public const string BudgetDelete = "Budget.Delete";

        public const string BudgetConsumptionRead = "BudgetConsumption.Read";

        public const string CategoryRead = "Category.Read";
        public const string CategoryCreate = "Category.Create";
        public const string CategoryUpdate = "Category.Update";
        public const string CategoryDelete = "Category.Delete";

        public const string CostCenterRead = "CostCenter.Read";
        public const string CostCenterCreate = "CostCenter.Create";
        public const string CostCenterUpdate = "CostCenter.Update";
        public const string CostCenterDelete = "CostCenter.Delete";

        public const string DepartmentRead = "Department.Read";
        public const string DepartmentCreate = "Department.Create";
        public const string DepartmentUpdate = "Department.Update";
        public const string DepartmentDelete = "Department.Delete";

        public const string IncotermRead = "Incoterm.Read";
        public const string IncotermCreate = "Incoterm.Create";
        public const string IncotermUpdate = "Incoterm.Update";
        public const string IncotermDelete = "Incoterm.Delete";

        public const string PlantRead = "Plant.Read";
        public const string PlantCreate = "Plant.Create";
        public const string PlantUpdate = "Plant.Update";
        public const string PlantDelete = "Plant.Delete";

        public const string ReportTypeRead = "ReportType.Read";
        public const string ReportTypeCreate = "ReportType.Create";
        public const string ReportTypeUpdate = "ReportType.Update";
        public const string ReportTypeDelete = "ReportType.Delete";

        public const string SupplierRead = "Supplier.Read";
        public const string SupplierCreate = "Supplier.Create";
        public const string SupplierUpdate = "Supplier.Update";
        public const string SupplierDelete = "Supplier.Delete";

        public const string UsoCFDIRead = "UsoCFDI.Read";
        public const string UsoCFDICreate = "UsoCFDI.Create";
        public const string UsoCFDIUpdate = "UsoCFDI.Update";
        public const string UsoCFDIDelete = "UsoCFDI.Delete";

        public const string WorkflowStepRead = "WorkflowStep.Read";
        public const string WorkflowStepCreate = "WorkflowStep.Create";
        public const string WorkflowStepUpdate = "WorkflowStep.Update";
        public const string WorkflowStepDelete = "WorkflowStep.Delete";

        public const string ReportAttachmentRead = "ReportAttachment.Read";
        public const string ReportAttachmentCreate = "ReportAttachment.Create";
        public const string ReportAttachmentDelete = "ReportAttachment.Delete";

        public static IEnumerable<string> All => new[]
        {
            ExpensesRead, ExpensesCreate, ExpensesUpdate, ExpensesDelete, ExpensesApprove,
            UsersManage, RolesManage, PermissionsManage,
            WorkflowRead, WorkflowCreate, WorkflowUpdate, WorkflowDelete,

            AccountProjectRead, AccountProjectCreate, AccountProjectUpdate, AccountProjectDelete,
            BudgetRead, BudgetCreate, BudgetUpdate, BudgetDelete,
            BudgetConsumptionRead,
            CategoryRead, CategoryCreate, CategoryUpdate, CategoryDelete,
            CostCenterRead, CostCenterCreate, CostCenterUpdate, CostCenterDelete,
            DepartmentRead, DepartmentCreate, DepartmentUpdate, DepartmentDelete,
            IncotermRead, IncotermCreate, IncotermUpdate, IncotermDelete,
            PlantRead, PlantCreate, PlantUpdate, PlantDelete,
            ReportTypeRead, ReportTypeCreate, ReportTypeUpdate, ReportTypeDelete,
            SupplierRead, SupplierCreate, SupplierUpdate, SupplierDelete,
            UsoCFDIRead, UsoCFDICreate, UsoCFDIUpdate, UsoCFDIDelete,
            WorkflowStepRead, WorkflowStepCreate, WorkflowStepUpdate, WorkflowStepDelete,
            ReportAttachmentRead, ReportAttachmentCreate, ReportAttachmentDelete
        };
    }
}
