using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Simpl.Expenses.Infrastructure.Persistence;
using System.Linq;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _dbName = System.Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
                SeedTestData(db);
            });
        }

        private static void SeedTestData(ApplicationDbContext context)
        {
            var roles = new[]
            {
                new Role { Name = "Admin" },
                new Role { Name = "User" }
            };
            context.Roles.AddRange(roles);

            var costCenters = new[]
            {
                new CostCenter { Name = "Cost Center 1", Code = "CC1", IsActive = true },
                new CostCenter { Name = "Cost Center 2", Code = "CC2", IsActive = true }
            };
            context.CostCenters.AddRange(costCenters);
            context.SaveChanges();

            var departments = new[]
            {
                new Department { Name = "IT", CostCenterId = costCenters[0].Id },
                new Department { Name = "HR", CostCenterId = costCenters[1].Id }
            };
            context.Departments.AddRange(departments);

            context.Categories.AddRange(
                new Category { Name = "Travel", Icon = "plane" },
                new Category { Name = "Office Supplies", Icon = "pencil" }
            );

            context.Suppliers.AddRange(
                new Supplier { Name = "Supplier 1", Rfc = "S1RFC", Address = "Address 1", Email = "supplier1@test.com", IsActive = true },
                new Supplier { Name = "Supplier 2", Rfc = "S2RFC", Address = "Address 2", Email = "supplier2@test.com", IsActive = true }
            );

            context.AccountProjects.AddRange(
                new AccountProject { Name = "Account Project 1", Code = "AP1", IsActive = true },
                new AccountProject { Name = "Account Project 2", Code = "AP2", IsActive = true }
            );

            context.Plants.AddRange(
                new Plant { Name = "Plant 1" },
                new Plant { Name = "Plant 2" }
            );

            context.ReportTypes.AddRange(
                new ReportType { Name = "Reimbursement" },
                new ReportType { Name = "Purchase Order" }
            );

            context.UsoCFDIs.AddRange(
                new UsoCFDI { Clave = "G01", Description = "Adquisición de mercancias" },
                new UsoCFDI { Clave = "G03", Description = "Gastos en general" }
            );

            context.Incoterms.AddRange(
                new Incoterm { Clave = "CIF", Description = "Cost, Insurance and Freight" },
                new Incoterm { Clave = "FOB", Description = "Free On Board" }
            );

            context.Users.AddRange(
                new User { Username = "admin", Email = "admin@test.com", PasswordHash = "test", RoleId = roles[0].Id, DepartmentId = departments[0].Id, IsActive = true },
                new User { Username = "user", Email = "user@test.com", PasswordHash = "test", RoleId = roles[1].Id, DepartmentId = departments[1].Id, IsActive = true }
            );

            context.SaveChanges();
        }
    }
}