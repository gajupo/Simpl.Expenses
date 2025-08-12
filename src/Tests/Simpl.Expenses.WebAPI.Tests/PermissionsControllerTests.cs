using System.Net;
using System.Net.Http.Json;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;
using Core.WebApi;

namespace Simpl.Expenses.WebAPI.Tests;

public class PermissionsControllerTests : IntegrationTestBase
{
    public PermissionsControllerTests(CustomWebApplicationFactory<Program> factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await LoginAsAdminAsync();
    }

    [Fact]
    public async Task GetPermissionById_WhenPermissionExists_ReturnsOk()
    {
        // Arrange
        var permission = new Permission { Name = "Test.Permission" };
        await AddAsync(permission);

        // Act
        var response = await _client.GetAsync($"/api/permissions/{permission.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var permissionDto = await response.Content.ReadFromJsonAsync<PermissionDto>();
        Assert.NotNull(permissionDto);
        Assert.Equal(permission.Id, permissionDto.Id);
        Assert.Equal(permission.Name, permissionDto.Name);
    }

    [Fact]
    public async Task GetAllPermissions_ReturnsOk()
    {
        // Arrange
        var deletedPermissions = await DeleteAllAsync<Permission>();
        await AddAsync(new Permission { Name = "Test.Permission.1" });
        await AddAsync(new Permission { Name = "Test.Permission.2" });

        // Act
        var response = await _client.GetAsync("/api/permissions");

        // Assert
        response.EnsureSuccessStatusCode();
        var permissions = await response.Content.ReadFromJsonAsync<List<PermissionDto>>();
        Assert.NotNull(permissions);
        Assert.Equal(2, permissions.Count);

        // restore deleted permissions
        foreach (var permission in deletedPermissions)
        {
            await AddAsync(permission);
        }
    }

    [Fact]
    public async Task CreatePermission_WithValidData_CreatesPermission()
    {
        // Arrange
        var createPermissionDto = new CreatePermissionDto { Name = "New.Permission" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/permissions", createPermissionDto);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var permissionDto = await response.Content.ReadFromJsonAsync<PermissionDto>();
        Assert.NotNull(permissionDto);
        Assert.Equal(createPermissionDto.Name, permissionDto.Name);
    }

    [Fact]
    public async Task UpdatePermission_WithValidData_UpdatesPermission()
    {
        // Arrange
        var permission = new Permission { Name = "Original.Permission" };
        await AddAsync(permission);
        var updatePermissionDto = new UpdatePermissionDto { Name = "Updated.Permission" };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/permissions/{permission.Id}", updatePermissionDto);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeletePermission_WhenPermissionExists_DeletesPermission()
    {
        // Arrange
        var permission = new Permission { Name = "Delete.Permission" };
        await AddAsync(permission);

        // Act
        var response = await _client.DeleteAsync($"/api/permissions/{permission.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
