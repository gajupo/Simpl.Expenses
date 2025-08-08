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

        public static IEnumerable<string> All => new[]
        {
            ExpensesRead, ExpensesCreate, ExpensesUpdate, ExpensesDelete, ExpensesApprove,
            UsersManage, RolesManage, PermissionsManage,
            WorkflowRead, WorkflowCreate, WorkflowUpdate, WorkflowDelete
        };
    }
}
