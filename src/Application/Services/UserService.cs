using AutoMapper;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Simpl.Expenses.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<UserPlant> _userPlantRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<UserPlant> userPlantRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userPlantRepository = userPlantRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int userId, CancellationToken cancellationToken = default)
        {
            var userProfile = await _userRepository.GetAll(cancellationToken)
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Name = u.Name,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name,
                    DepartmentId = u.DepartmentId,
                    DepartmentName = u.Department.Name,
                    CostCenterId = u.Department.CostCenter.Id,
                    CostCenterName = u.Department.CostCenter.Name,
                    ReportsToId = u.ReportsToId,
                    Permissions = u.Role.RolePermissions.Select(rp => rp.Permission.Name)
                        .Union(u.UserPermissions.Where(up => up.IsGranted).Select(up => up.Permission.Name))
                        .Distinct()
                        .ToArray()
                })
                .FirstOrDefaultAsync(cancellationToken);
            var userPlants = await _userPlantRepository.GetAll(cancellationToken)
                .Where(up => up.UserId == userId)
                .Select(up => new UserPlantDto
                {
                    PlantId = up.Plant.Id,
                    PlantName = up.Plant.Name
                })
                .ToListAsync(cancellationToken);

            if (userProfile != null)
            {
                userProfile.UserPlants = userPlants;
            }

            return userProfile;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetAll(cancellationToken)
                .AsTracking()
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Name = u.Name,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name,
                    DepartmentId = u.DepartmentId,
                    DepartmentName = u.Department.Name,
                    ReportsToId = u.ReportsToId,
                    IsActive = u.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetAll(cancellationToken)
                .AsTracking()
                .Include(u => u.Role)
                .Include(u => u.Department)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Name = u.Name,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name,
                    DepartmentId = u.DepartmentId,
                    DepartmentName = u.Department.Name,
                    ReportsToId = u.ReportsToId,
                    IsActive = u.IsActive
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<User>(createUserDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
            await _userRepository.AddAsync(user, cancellationToken);

            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null) return;

            _mapper.Map(updateUserDto, user);

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user != null)
            {
                await _userRepository.RemoveAsync(user, cancellationToken);
            }
        }
    }
}