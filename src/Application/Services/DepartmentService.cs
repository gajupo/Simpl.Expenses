using AutoMapper;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IGenericRepository<Department> departmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _departmentRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto, CancellationToken cancellationToken = default)
        {
            var department = _mapper.Map<Department>(createDepartmentDto);
            await _departmentRepository.AddAsync(department, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task UpdateDepartmentAsync(int id, UpdateDepartmentDto updateDepartmentDto, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (department == null) return;

            _mapper.Map(updateDepartmentDto, department);

            await _departmentRepository.UpdateAsync(department, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);
            if (department != null)
            {
                await _departmentRepository.RemoveAsync(department, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
