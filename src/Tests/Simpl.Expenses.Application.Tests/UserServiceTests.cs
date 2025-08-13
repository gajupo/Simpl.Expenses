
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Application.Services;
using Simpl.Expenses.Domain.Entities;
using Simpl.Expenses.Infrastructure.Persistence;
using Simpl.Expenses.Infrastructure.Repositories;

namespace Simpl.Expenses.Application.Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<UserPlant> _userPlantRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test run
                .Options;

            _context = new ApplicationDbContext(options);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();

            _userRepository = new GenericRepository<User>(_context);
            _userPlantRepository = new GenericRepository<UserPlant>(_context);
            _userService = new UserService(_userRepository, _userPlantRepository, _mapper);
        }

        private void SeedDatabase()
        {
            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            };
            _context.Roles.AddRange(roles);

            var costCenters = new List<CostCenter>
            {
                new CostCenter { Id = 1, Name = "Test Cost Center", Code = "TCC", IsActive = true }
            };
            _context.CostCenters.AddRange(costCenters);

            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "IT", CostCenterId = 1 },
                new Department { Id = 2, Name = "HR", CostCenterId = 1 }
            };
            _context.Departments.AddRange(departments);

            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Name = "User One", Email = "user1@test.com", PasswordHash = "hash1", IsActive = true, DepartmentId = 1, RoleId = 1 },
                new User { Id = 2, Username = "user2", Name = "User Two", Email = "user2@test.com", PasswordHash = "hash2", IsActive = true, DepartmentId = 1, RoleId = 1 }
            };
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            SeedDatabase();

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("user1", result.Username);
            Assert.Equal("Admin", result.RoleName);
            Assert.Equal("IT", result.DepartmentName);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUserDtos()
        {
            // Arrange
            SeedDatabase();

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            var user1 = result.First(u => u.Id == 1);
            Assert.Equal("Admin", user1.RoleName);
            Assert.Equal("IT", user1.DepartmentName);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "newuser", Name = "New User", Email = "new@user.com", Password = "password123", DepartmentId = 1, RoleId = 1 };

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAll_ShouldSupportProjection()
        {
            // Arrange
            SeedDatabase();

            // Act
            var projectedUsers = _userRepository.GetAll()
                .Select(u => new { u.Id, u.Username })
                .ToList();

            // Assert
            Assert.NotNull(projectedUsers);
            Assert.Equal(2, projectedUsers.Count);
            Assert.Equal(1, projectedUsers[0].Id);
            Assert.Equal("user1", projectedUsers[0].Username);
            Assert.Null(projectedUsers[0].GetType().GetProperty("Email"));
            Assert.Null(projectedUsers[0].GetType().GetProperty("PasswordHash"));
        }

        [Fact]
        public async Task CreateUserAsync_ShouldPersistUserToDatabase_AndIncrementUserCount()
        {
            // Arrange
            SeedDatabase();
            var initialUserCount = await _context.Users.CountAsync();
            var createUserDto = new CreateUserDto
            {
                Username = "persisteduser",
                Name = "Persisted User",
                Email = "persisted@user.com",
                Password = "securepassword123",
                DepartmentId = 1,
                RoleId = 1
            };

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert - Verify the returned DTO
            Assert.NotNull(result);
            Assert.Equal("persisteduser", result.Username);
            Assert.Equal("persisted@user.com", result.Email);
            Assert.True(result.Id > 0);

            // Assert - Verify the user was actually saved to the database
            var finalUserCount = await _context.Users.CountAsync();
            Assert.Equal(initialUserCount + 1, finalUserCount);

            // Assert - Verify the user can be retrieved from the database
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "persisteduser");
            Assert.NotNull(savedUser);
            Assert.Equal("persisteduser", savedUser.Username);
            Assert.Equal("persisted@user.com", savedUser.Email);
            Assert.True(BCrypt.Net.BCrypt.Verify("securepassword123", savedUser.PasswordHash));
            Assert.Equal(1, savedUser.DepartmentId);
            Assert.Equal(1, savedUser.RoleId);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
