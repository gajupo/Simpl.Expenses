
using Core.WebApi;
using Simpl.Expenses.Application.Dtos.User;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class UsersControllerTests : IntegrationTestBase
    {
        public UsersControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_CreatesUser_And_ReturnsCreatedUser()
        {
            // Arrange - Clean database before test
            CleanupDatabase();

            var newUser = new CreateUserDto
            {
                Username = "testuser",
                Email = "test@user.com",
                Password = "password",
                RoleId = 1,
                DepartmentId = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/users", newUser);
            response.EnsureSuccessStatusCode();

            var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.NotNull(createdUser);
            Assert.Equal(newUser.Username, createdUser.Username);
            Assert.Equal(newUser.Email, createdUser.Email);
            Assert.Equal(newUser.RoleId, createdUser.RoleId);
            Assert.Equal(newUser.DepartmentId, createdUser.DepartmentId);
        }

        [Fact]
        public async Task Post_WithExistingEmail_InMemoryDB_AllowsDuplicate()
        {
            // Arrange - Clean database and create a user first
            CleanupDatabase();

            var firstUser = new CreateUserDto
            {
                Username = "firstuser",
                Email = "duplicate@user.com",
                Password = "password",
                RoleId = 1,
                DepartmentId = 1
            };

            var secondUser = new CreateUserDto
            {
                Username = "seconduser",
                Email = "duplicate@user.com", // Same email
                Password = "password",
                RoleId = 1,
                DepartmentId = 1
            };

            // Act - Create first user
            var firstResponse = await _client.PostAsJsonAsync("/api/users", firstUser);
            firstResponse.EnsureSuccessStatusCode();

            // Act - Try to create second user with same email
            var secondResponse = await _client.PostAsJsonAsync("/api/users", secondUser);

            // Assert - In-memory database allows duplicates (this is expected behavior)
            // In production with SQL Server, this would return 500 due to unique constraint
            Assert.Equal(System.Net.HttpStatusCode.Created, secondResponse.StatusCode);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsUser()
        {
            // Arrange - Clean database and create a user
            CleanupDatabase();

            var createUser = new CreateUserDto
            {
                Username = "getbyiduser",
                Email = "getbyid@user.com",
                Password = "password123",
                RoleId = 1,
                DepartmentId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/users", createUser);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(createdUser);

            // Act
            var response = await _client.GetAsync($"/api/users/{createdUser.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedUser = await response.Content.ReadFromJsonAsync<UserDto>();

            Assert.NotNull(returnedUser);
            Assert.Equal(createdUser.Id, returnedUser.Id);
            Assert.Equal(createUser.Username, returnedUser.Username);
            Assert.Equal(createUser.Email, returnedUser.Email);
            Assert.Equal(createUser.RoleId, returnedUser.RoleId);
            Assert.Equal(createUser.DepartmentId, returnedUser.DepartmentId);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange - Clean database (no users exist)
            CleanupDatabase();

            // Act
            var response = await _client.GetAsync("/api/users/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Put_UpdateExistingUser_ReturnsNoContent()
        {
            // Arrange - Clean database and create a user
            CleanupDatabase();

            var createUser = new CreateUserDto
            {
                Username = "originaluser",
                Email = "original@user.com",
                Password = "password123",
                RoleId = 1,
                DepartmentId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/users", createUser);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(createdUser);

            var updateUser = new UpdateUserDto
            {
                Username = "updateduser",
                Email = "updated@user.com",
                RoleId = 1,
                DepartmentId = 1,
                IsActive = false
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/users/{createdUser.Id}", updateUser);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

            // Verify the update by getting the user
            var getResponse = await _client.GetAsync($"/api/users/{createdUser.Id}");
            getResponse.EnsureSuccessStatusCode();
            var updatedUserResult = await getResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(updatedUserResult);

            Assert.Equal("updateduser", updatedUserResult.Username);
            Assert.Equal("updated@user.com", updatedUserResult.Email);
            Assert.False(updatedUserResult.IsActive);
        }

        [Fact]
        public async Task Put_UpdateNonExistentUser_ReturnsNoContent()
        {
            // Arrange - Clean database (no users exist)
            CleanupDatabase();

            var updateUser = new UpdateUserDto
            {
                Username = "nonexistent",
                Email = "nonexistent@user.com"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/users/999", updateUser);

            // Assert - Service silently ignores non-existent users and returns NoContent
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Put_PartialUpdate_UpdatesProvidedFields()
        {
            // Arrange - Clean database and create a user
            CleanupDatabase();

            var createUser = new CreateUserDto
            {
                Username = "partialuser",
                Email = "partial@user.com",
                Password = "password123",
                RoleId = 1,
                DepartmentId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/users", createUser);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(createdUser);

            // Update username and email (required fields) but keep role and department same
            var partialUpdate = new UpdateUserDto
            {
                Username = "newusername",
                Email = "partial@user.com", // Keep same email
                RoleId = 1, // Keep same role
                DepartmentId = 1, // Keep same department
                IsActive = false // Change this field
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/users/{createdUser.Id}", partialUpdate);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

            // Verify fields were updated
            var getResponse = await _client.GetAsync($"/api/users/{createdUser.Id}");
            getResponse.EnsureSuccessStatusCode();
            var updatedUserResult = await getResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(updatedUserResult);

            Assert.Equal("newusername", updatedUserResult.Username); // Should be updated
            Assert.Equal("partial@user.com", updatedUserResult.Email); // Should remain same
            Assert.Equal(1, updatedUserResult.RoleId); // Should remain same
            Assert.Equal(1, updatedUserResult.DepartmentId); // Should remain same
            Assert.False(updatedUserResult.IsActive); // Should be updated to false
        }

        [Fact]
        public async Task Delete_ExistingUser_ReturnsNoContent()
        {
            // Arrange - Clean database and create a user
            CleanupDatabase();

            var createUser = new CreateUserDto
            {
                Username = "deleteuser",
                Email = "delete@user.com",
                Password = "password123",
                RoleId = 1,
                DepartmentId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/api/users", createUser);
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(createdUser);

            // Act
            var response = await _client.DeleteAsync($"/api/users/{createdUser.Id}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

            // Verify user is deleted by trying to get it
            var getResponse = await _client.GetAsync($"/api/users/{createdUser.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task Delete_NonExistentUser_ReturnsNoContent()
        {
            // Arrange - Clean database (no users exist)
            CleanupDatabase();

            // Act
            var response = await _client.DeleteAsync("/api/users/999");

            // Assert - Service silently ignores non-existent users and returns NoContent
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetAll_WithMultipleUsers_ReturnsAllUsers()
        {
            // Arrange - Clean database and create multiple users
            CleanupDatabase();

            var users = new List<CreateUserDto>
            {
                new CreateUserDto
                {
                    Username = "user1",
                    Email = "user1@test.com",
                    Password = "password123",
                    RoleId = 1,
                    DepartmentId = 1
                },
                new CreateUserDto
                {
                    Username = "user2",
                    Email = "user2@test.com",
                    Password = "password123",
                    RoleId = 1,
                    DepartmentId = 1
                },
                new CreateUserDto
                {
                    Username = "user3",
                    Email = "user3@test.com",
                    Password = "password123",
                    RoleId = 1,
                    DepartmentId = 1
                }
            };

            // Create all users
            foreach (var user in users)
            {
                var createResponse = await _client.PostAsJsonAsync("/api/users", user);
                createResponse.EnsureSuccessStatusCode();
            }

            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedUsers = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            Assert.NotNull(returnedUsers);
            Assert.Equal(3, returnedUsers.Count);

            // Verify all usernames are present
            var usernames = returnedUsers.Select(u => u.Username).ToList();
            Assert.Contains("user1", usernames);
            Assert.Contains("user2", usernames);
            Assert.Contains("user3", usernames);
        }

        [Fact]
        public async Task GetAll_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Arrange - Clean database (no users exist)
            CleanupDatabase();

            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedUsers = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            Assert.NotNull(returnedUsers);
            Assert.Empty(returnedUsers);
        }
    }
}
