using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IGenericRepository<Permission> _permissionRepository;
    private readonly IMapper _mapper;

    public PermissionService(IGenericRepository<Permission> permissionRepository, IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.GetAll(cancellationToken)
            .ProjectTo<PermissionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<PermissionDto?> GetPermissionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.GetAll(cancellationToken)
            .Where(p => p.Id == id)
            .ProjectTo<PermissionDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto createPermissionDto, CancellationToken cancellationToken = default)
    {
        var permission = _mapper.Map<Permission>(createPermissionDto);
        await _permissionRepository.AddAsync(permission, cancellationToken);
        return _mapper.Map<PermissionDto>(permission);
    }

    public async Task UpdatePermissionAsync(int id, UpdatePermissionDto updatePermissionDto, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetByIdAsync(id, cancellationToken);
        if (permission == null) return;

        _mapper.Map(updatePermissionDto, permission);

        await _permissionRepository.UpdateAsync(permission, cancellationToken);
    }

    public async Task DeletePermissionAsync(int id, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetByIdAsync(id, cancellationToken);
        if (permission != null)
        {
            await _permissionRepository.RemoveAsync(permission, cancellationToken);
        }
    }
}
