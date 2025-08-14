
using AutoMapper;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Application.Dtos.Report;
using Simpl.Expenses.Application.Dtos.ReportState;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<Role, RoleDto>();
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();

            CreateMap<Department, DepartmentDto>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<Supplier, SupplierDto>();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>();

            CreateMap<CostCenter, CostCenterDto>();
            CreateMap<CreateCostCenterDto, CostCenter>();
            CreateMap<UpdateCostCenterDto, CostCenter>();

            CreateMap<AccountProject, AccountProjectDto>();
            CreateMap<CreateAccountProjectDto, AccountProject>();
            CreateMap<UpdateAccountProjectDto, AccountProject>();

            CreateMap<Plant, PlantDto>();
            CreateMap<CreatePlantDto, Plant>();
            CreateMap<UpdatePlantDto, Plant>();

            CreateMap<ReportType, ReportTypeDto>()
                .ForMember(dest => dest.DefaultWorkflowName, opt => opt.MapFrom(src => src.DefaultWorkflow != null ? src.DefaultWorkflow.Name : null));
            CreateMap<CreateReportTypeDto, ReportType>();
            CreateMap<UpdateReportTypeDto, ReportType>();

            CreateMap<UsoCFDI, UsoCFDIDto>();
            CreateMap<CreateUsoCFDIDto, UsoCFDI>();
            CreateMap<UpdateUsoCFDIDto, UsoCFDI>();

            CreateMap<Incoterm, IncotermDto>();
            CreateMap<CreateIncotermDto, Incoterm>();
            CreateMap<UpdateIncotermDto, Incoterm>();

            CreateMap<Workflow, WorkflowDto>()
                .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps));
            CreateMap<CreateWorkflowDto, Workflow>();
            CreateMap<UpdateWorkflowDto, Workflow>();

            CreateMap<WorkflowStep, WorkflowStepDto>();

            CreateMap<Permission, PermissionDto>();
            CreateMap<CreatePermissionDto, Permission>();
            CreateMap<UpdatePermissionDto, Permission>();

            CreateMap<UserPermission, UserPermissionDto>();
            CreateMap<CreateUserPermissionDto, UserPermission>();
            CreateMap<UpdateUserPermissionDto, UserPermission>();
            CreateMap<CreateWorkflowStepDto, WorkflowStep>();
            CreateMap<UpdateWorkflowStepDto, WorkflowStep>();

            CreateMap<Report, ReportDto>().ReverseMap();
            CreateMap<CreateReportDto, Report>();
            CreateMap<UpdateReportDto, Report>()
                .ForMember(dest => dest.PurchaseOrderDetail, opt => opt.Ignore())
                .ForMember(dest => dest.AdvancePaymentDetail, opt => opt.Ignore())
                .ForMember(dest => dest.ReimbursementDetail, opt => opt.Ignore());

            CreateMap<Report, ReportOverviewDto>()
                .ForMember(dest => dest.ReportTypeName, opt => opt.MapFrom(src => src.ReportType.Name))
                .ForMember(dest => dest.PlantName, opt => opt.MapFrom(src => src.Plant.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.AccountProjectName, opt => opt.MapFrom(src => src.AccountProject != null ? src.AccountProject.Name : null));

            CreateMap<PurchaseOrderDetail, PurchaseOrderDetailDto>().ReverseMap();
            CreateMap<CreatePurchaseOrderDetailDto, PurchaseOrderDetail>();
            CreateMap<UpdatePurchaseOrderDetailDto, PurchaseOrderDetail>();

            CreateMap<AdvancePaymentDetail, AdvancePaymentDetailDto>().ReverseMap();
            CreateMap<CreateAdvancePaymentDetailDto, AdvancePaymentDetail>();
            CreateMap<UpdateAdvancePaymentDetailDto, AdvancePaymentDetail>();

            CreateMap<ReimbursementDetail, ReimbursementDetailDto>().ReverseMap();
            CreateMap<CreateReimbursementDetailDto, ReimbursementDetail>();
            CreateMap<UpdateReimbursementDetailDto, ReimbursementDetail>();

            CreateMap<Budget, BudgetDto>();
            CreateMap<CreateBudgetDto, Budget>();
            CreateMap<UpdateBudgetDto, Budget>();

            CreateMap<BudgetConsumption, BudgetConsumptionDto>()
                .ForMember(dest => dest.ReportType, opt => opt.MapFrom(src => src.Report.ReportType.Name))
                .ForMember(dest => dest.ReportAmount, opt => opt.MapFrom(src => src.Report.Amount))
                .ForMember(dest => dest.PlantName, opt => opt.MapFrom(src => src.Report.Plant.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Report.Category.Name));

            CreateMap<ReportState, ReportStateDto>().ReverseMap();
            CreateMap<CreateReportStateDto, ReportState>();

            CreateMap<ApprovalLog, ApprovalLogDto>().ReverseMap();
            CreateMap<CreateApprovalLogDto, ApprovalLog>();
            CreateMap<UpdateApprovalLogDto, ApprovalLog>();

            CreateMap<ReportAttachment, ReportAttachmentDto>();
        }
    }
}
