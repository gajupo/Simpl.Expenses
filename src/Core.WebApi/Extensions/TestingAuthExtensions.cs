using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Simpl.Expenses.Domain.Constants;
using Core.WebApi.Middlewares;

namespace Core.WebApi.Extensions
{
    public static class TestingAuthExtensions
    {
        // Registers a test auth scheme and makes it the default when running under the "Testing" environment
        public static IServiceCollection AddTestingAuthentication(this IServiceCollection services, IHostEnvironment env)
        {
            if (env.IsEnvironment("Testing"))
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
            }

            return services;
        }

        // Optionally injects a synthetic Admin principal with all permissions in Testing
        public static IApplicationBuilder UseTestingIdentity(this IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsEnvironment("Testing"))
            {
                app.Use(async (ctx, next) =>
                {
                    if (ctx.User?.Identity is not { IsAuthenticated: true })
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, "1"),
                            new Claim(ClaimTypes.Name, "test-user"),
                            new Claim(ClaimTypes.Role, "Admin")
                        };
                        foreach (var p in PermissionCatalog.All)
                        {
                            claims.Add(new Claim("permission", p));
                        }
                        ctx.User = new ClaimsPrincipal(new ClaimsIdentity(claims, TestAuthHandler.SchemeName));
                    }
                    await next();
                });
            }

            return app;
        }
    }
}
