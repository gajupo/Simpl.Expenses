using Microsoft.Extensions.DependencyInjection;
using Simpl.Expenses.Infrastructure.Persistence;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Core.WebApi;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Simpl.Expenses.WebAPI.Tests
{
    public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        protected readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly ApplicationDbContext _context;
        private readonly CustomWebApplicationFactory<Program> _factory;

        private record TokenResponse(string token);
        protected IntegrationTestBase(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _scope = factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected async Task LoginAsAdminAsync()
        {
            var request = new { Username = "padmin", Password = "password" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);
            response.EnsureSuccessStatusCode();

            // create a record tokenReponse with a token property to store the deserialized response from api
            TokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.token);
        }

        protected async Task<T> AddAsync<T>(T entity) where T : class
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        protected Task<T> RemoveEntityAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChangesAsync();
            return Task.FromResult(entity);
        }

        protected Task<List<T>> DeleteAllAsync<T>() where T : class
        {
            var entities = _context.Set<T>().ToList();
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChangesAsync();
            return Task.FromResult(entities);
        }

        public Task DisposeAsync()
        {
            _scope?.Dispose();
            _client?.Dispose();
            return Task.CompletedTask;
        }
    }
}
