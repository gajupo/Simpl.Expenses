
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
            CreateMap<User, UserDto>();
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

            CreateMap<ReportType, ReportTypeDto>();
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
            CreateMap<CreateWorkflowStepDto, WorkflowStep>();
            CreateMap<UpdateWorkflowStepDto, WorkflowStep>();

            CreateMap<Report, ReportDto>().ReverseMap();
            CreateMap<CreateReportDto, Report>();
            CreateMap<UpdateReportDto, Report>()
                .ForMember(dest => dest.PurchaseOrderDetail, opt => opt.Ignore())
                .ForMember(dest => dest.AdvancePaymentDetail, opt => opt.Ignore())
                .ForMember(dest => dest.ReimbursementDetail, opt => opt.Ignore());

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
        }
    }
}
