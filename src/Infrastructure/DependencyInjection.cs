
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Infrastructure.Persistence;
using Simpl.Expenses.Infrastructure.Repositories;

namespace Simpl.Expenses.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            if (env.IsEnvironment("Testing"))
            {
                // ONLY InMemory in test runs
                services.AddDbContext<ApplicationDbContext>(opts =>
                    opts.UseInMemoryDatabase("InMemoryDbForTesting"));
            }
            else
            {
                // Normal SQL Server in Dev / Prod
                var conn = configuration
                    .GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Missing DefaultConnection");

                services.AddDbContext<ApplicationDbContext>(opts =>
                    opts.UseSqlServer(
                        conn,
                        sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                    ));
            }

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}
