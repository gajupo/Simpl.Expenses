
using Microsoft.Extensions.DependencyInjection;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Application.Services;

namespace Simpl.Expenses.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ICostCenterService, CostCenterService>();
            services.AddScoped<IAccountProjectService, AccountProjectService>();
            services.AddScoped<IPlantService, PlantService>();
            services.AddScoped<IReportTypeService, ReportTypeService>();
            services.AddScoped<IUsoCFDIService, UsoCFDIService>();
            services.AddScoped<IIncotermService, IncotermService>();
            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddScoped<IWorkflowStepService, WorkflowStepService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<IBudgetConsumptionService, BudgetConsumptionService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportStateService, ReportStateService>();
            services.AddScoped<IApprovalLogService, ApprovalLogService>();
            return services;
        }
    }
}
