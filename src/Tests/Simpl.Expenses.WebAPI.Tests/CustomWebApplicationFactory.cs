
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Simpl.Expenses.Infrastructure.Persistence;
using Simpl.Expenses.Domain.Entities;
using System.Linq;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Build service provider to seed data after all services are registered
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                SeedTestData(context);
            });
        }

        private static void SeedTestData(ApplicationDbContext context)
        {
            // Clear existing data
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed required reference data
            var role = new Role { Id = 1, Name = "TestRole" };
            var department = new Department { Id = 1, Name = "TestDepartment" };

            context.Roles.Add(role);
            context.Departments.Add(department);
            context.SaveChanges();
        }
    }
}
