using Microsoft.Extensions.DependencyInjection;
using Simpl.Expenses.Infrastructure.Persistence;
using System.Net.Http;
using Xunit;
using Core.WebApi;

namespace Simpl.Expenses.WebAPI.Tests
{
    public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected readonly HttpClient _client;
        protected readonly CustomWebApplicationFactory<Program> _factory;
        protected readonly ApplicationDbContext _context;

        protected IntegrationTestBase(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();

            // Get a scope for database operations
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        protected void CleanupDatabase()
        {
            // Clear all users but keep reference data (roles, departments)
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
        }

        public virtual void Dispose()
        {
            _client?.Dispose();
            _context?.Dispose();
        }
    }
}
