
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Simpl.Expenses.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Get the directory of the current project (Infrastructure)
            var infrastructureDirectory = Directory.GetCurrentDirectory();

            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

            // 2. Construye IConfiguration cargando appsettings.json y el env específico
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // 3. Configura DbContextOptions con la cadena de conexión adecuada
        var optsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connStr = config.GetConnectionString("DefaultConnection");
        optsBuilder.UseSqlServer(connStr);

        return new ApplicationDbContext(optsBuilder.Options);
        }
    }
}
