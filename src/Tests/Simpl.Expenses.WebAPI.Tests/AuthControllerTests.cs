using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests.Controllers;

public class AuthControllerTests : IntegrationTestBase
{
    public AuthControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var request = new { Username = "padmin", Password = "password" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);
        var jwt = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var request = new { Username = "testuser", Password = "wrongpassword" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithPermission_ReturnsOk()
    {
        // Arrange
        var token = await GetJwtToken("padmin", "password");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/workflows");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ProtectedEndpoint_WithRoleButNoPermissions_ReturnsForbidden()
    {
        // Arrange
        var role = new Role { Name = "NoPermissions" };
        await AddAsync(role);

        var user = new User { Username = "nopermissionsuser", Email = "nopermissions@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), RoleId = role.Id, DepartmentId = 1, IsActive = true };
        await AddAsync(user);

        var token = await GetJwtToken("nopermissionsuser", "password");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/workflows");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    private async Task<string> GetJwtToken(string userName, string password)
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { Username = userName, Password = password });
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var token = JsonDocument.Parse(responseContent).RootElement.GetProperty("token").GetString();
        return token;
    }
}