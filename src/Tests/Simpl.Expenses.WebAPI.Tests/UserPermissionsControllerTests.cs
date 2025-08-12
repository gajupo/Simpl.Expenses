using System.Net;
using System.Net.Http.Json;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;
using Core.WebApi;

namespace Simpl.Expenses.WebAPI.Tests;

public class UserPermissionsControllerTests : IntegrationTestBase
{
    public UserPermissionsControllerTests(CustomWebApplicationFactory<Program> factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await LoginAsAdminAsync();
    }

    private async Task<(User user, Permission permission)> CreateUserAndPermissionAsync()
    {
        var user = new User { Username = "testuser", Email = "test@test.com", PasswordHash = "..." };
        await AddAsync(user);

        var permission = new Permission { Name = "Test.Permission" };
        await AddAsync(permission);

        return (user, permission);
    }

    [Fact]
    public async Task GetPermissionsForUser_ReturnsOk()
    {
        // Arrange
        var (user, permission) = await CreateUserAndPermissionAsync();
        await AddAsync(new UserPermission { UserId = user.Id, PermissionId = permission.Id });

        // Act
        var response = await _client.GetAsync($"/api/userspermissions/{user.Id}/permissions");

        // Assert
        response.EnsureSuccessStatusCode();
        var permissions = await response.Content.ReadFromJsonAsync<List<UserPermissionDto>>();
        Assert.NotNull(permissions);
        Assert.Single(permissions);
        Assert.Equal(permission.Id, permissions[0].PermissionId);
    }

    [Fact]
    public async Task AddPermissionToUser_WithValidData_AddsPermission()
    {
        // Arrange
        var (user, permission) = await CreateUserAndPermissionAsync();
        var createUserPermissionDto = new CreateUserPermissionDto { UserId = user.Id, PermissionId = permission.Id };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/userspermissions/{user.Id}/permissions", createUserPermissionDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var permissionDto = await response.Content.ReadFromJsonAsync<UserPermissionDto>();
        Assert.NotNull(permissionDto);
        Assert.Equal(user.Id, permissionDto.UserId);
        Assert.Equal(permission.Id, permissionDto.PermissionId);
    }

    [Fact]
    public async Task UpdatePermissionForUser_WithValidData_UpdatesPermission()
    {
        // Arrange
        var (user, permission) = await CreateUserAndPermissionAsync();
        await AddAsync(new UserPermission { UserId = user.Id, PermissionId = permission.Id, IsGranted = true });
        var updateUserPermissionDto = new UpdateUserPermissionDto { IsGranted = false };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/userspermissions/{user.Id}/permissions/{permission.Id}", updateUserPermissionDto);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task RemovePermissionFromUser_WhenPermissionExists_RemovesPermission()
    {
        // Arrange
        var (user, permission) = await CreateUserAndPermissionAsync();
        await AddAsync(new UserPermission { UserId = user.Id, PermissionId = permission.Id });

        // Act
        var response = await _client.DeleteAsync($"/api/userspermissions/{user.Id}/permissions/{permission.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
