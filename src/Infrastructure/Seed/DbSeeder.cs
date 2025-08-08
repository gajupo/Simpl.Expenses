using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Simpl.Expenses.Domain.Constants;
using Simpl.Expenses.Domain.Entities;
using Simpl.Expenses.Infrastructure.Persistence;

namespace Simpl.Expenses.Infrastructure.Seed
{
    public static class DbSeeder
    {
        public static async Task EnsureSeededAsync(IServiceProvider services, CancellationToken ct = default)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;
            var logger = provider.GetService<ILoggerFactory>()?.CreateLogger("DbSeeder");
            var context = provider.GetRequiredService<ApplicationDbContext>();

            // Apply migrations only for relational providers; InMemory can't migrate
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync(ct);
            }
            else
            {
                await context.Database.EnsureCreatedAsync(ct);
            }

            // Seed permissions from catalog
            var existingPermissionNames = await context.Permissions
                .Select(p => p.Name)
                .ToListAsync(ct);

            var newPermissions = PermissionCatalog.All
                .Except(existingPermissionNames)
                .Select(name => new Permission { Name = name })
                .ToList();

            if (newPermissions.Count > 0)
            {
                await context.Permissions.AddRangeAsync(newPermissions, ct);
                await context.SaveChangesAsync(ct);
                logger?.LogInformation("Seeded {Count} permissions", newPermissions.Count);
            }

            // Ensure Admin role exists
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin", ct);
            if (adminRole == null)
            {
                adminRole = new Role { Name = "Admin" };
                await context.Roles.AddAsync(adminRole, ct);
                await context.SaveChangesAsync(ct);
                logger?.LogInformation("Created Admin role");
            }

            // Assign all permissions to Admin role
            var adminPermissionNames = await context.RolePermissions
                .Where(rp => rp.RoleId == adminRole.Id)
                .Select(rp => rp.Permission.Name)
                .ToListAsync(ct);

            var missingForAdmin = PermissionCatalog.All.Except(adminPermissionNames).ToList();
            if (missingForAdmin.Count > 0)
            {
                var missingPermissionEntities = await context.Permissions
                    .Where(p => missingForAdmin.Contains(p.Name))
                    .Select(p => new { p.Id })
                    .ToListAsync(ct);

                foreach (var p in missingPermissionEntities)
                {
                    await context.RolePermissions.AddAsync(new RolePermission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = p.Id
                    }, ct);
                }

                await context.SaveChangesAsync(ct);
                logger?.LogInformation("Granted {Count} permissions to Admin role", missingForAdmin.Count);
            }

            logger?.LogInformation("Database seeding completed");
        }
    }
}
