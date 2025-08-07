using AutoMapper;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IGenericRepository<Role> roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<RoleDto> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _roleRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto, CancellationToken cancellationToken = default)
        {
            var role = _mapper.Map<Role>(createRoleDto);
            await _roleRepository.AddAsync(role, cancellationToken);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto, CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.GetByIdAsync(id, cancellationToken);
            if (role == null) return;

            _mapper.Map(updateRoleDto, role);

            await _roleRepository.UpdateAsync(role, cancellationToken);
        }

        public async Task DeleteRoleAsync(int id, CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.GetByIdAsync(id, cancellationToken);
            if (role != null)
            {
                await _roleRepository.RemoveAsync(role, cancellationToken);
            }
        }
    }
}
