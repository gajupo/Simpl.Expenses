using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Application.Services;

public class UserPermissionService : IUserPermissionService
{
    private readonly IGenericRepository<UserPermission> _userPermissionRepository;
    private readonly IMapper _mapper;

    public UserPermissionService(IGenericRepository<UserPermission> userPermissionRepository, IMapper mapper)
    {
        _userPermissionRepository = userPermissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserPermissionDto>> GetPermissionsForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _userPermissionRepository.GetAll(cancellationToken)
            .Where(up => up.UserId == userId)
            .ProjectTo<UserPermissionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserPermissionDto> AddPermissionToUserAsync(CreateUserPermissionDto createUserPermissionDto, CancellationToken cancellationToken = default)
    {
        var userPermission = _mapper.Map<UserPermission>(createUserPermissionDto);
        await _userPermissionRepository.AddAsync(userPermission, cancellationToken);
        return _mapper.Map<UserPermissionDto>(userPermission);
    }

    public async Task UpdatePermissionForUserAsync(int userId, int permissionId, UpdateUserPermissionDto updateUserPermissionDto, CancellationToken cancellationToken = default)
    {
        var userPermission = await _userPermissionRepository.GetAll(cancellationToken)
            .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId, cancellationToken);

        if (userPermission == null) return;

        _mapper.Map(updateUserPermissionDto, userPermission);

        await _userPermissionRepository.UpdateAsync(userPermission, cancellationToken);
    }

    public async Task RemovePermissionFromUserAsync(int userId, int permissionId, CancellationToken cancellationToken = default)
    {
        var userPermission = await _userPermissionRepository.GetAll(cancellationToken)
            .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId, cancellationToken);

        if (userPermission != null)
        {
            await _userPermissionRepository.RemoveAsync(userPermission, cancellationToken);
        }
    }
}
