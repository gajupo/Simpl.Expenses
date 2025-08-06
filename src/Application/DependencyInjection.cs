
using Microsoft.Extensions.DependencyInjection;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Application.Services;

namespace Simpl.Expenses.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
