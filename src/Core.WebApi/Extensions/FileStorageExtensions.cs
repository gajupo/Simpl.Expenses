using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simpl.Expenses.Application.Configuration;

namespace Core.WebApi.Extensions
{
    public static class FileStorageExtensions
    {
        public static IServiceCollection ConfigureFileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure file upload size limit
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
                options.ValueLengthLimit = 10 * 1024 * 1024;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            // Bind config
            services.Configure<FileStorageSettings>(configuration.GetSection("FileStorage"));

            return services;
        }
    }
}
