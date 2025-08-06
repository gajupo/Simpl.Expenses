using Core.WebApi.Extensions;
using Serilog;
using Simpl.Expenses.Infrastructure;
namespace Core.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            await RunApplicationAsync(builder);
        }

        public static WebApplicationBuilder CreateHostBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            return builder;
        }

        public static async Task RunApplicationAsync(
        WebApplicationBuilder builder,
        CancellationToken cancellationToken = default,
        Serilog.ILogger? logger = null
        )
        {
            // Use provided logger or create default
            Log.Logger = logger ?? new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Log.Information("Starting up Core API");
            try
            {
                var app = builder.Build();
                await ConfigureApplication(app, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                throw;
            }
            finally
            {
                Log.Information("Shutting down");
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // configure Serilog
            builder.ConfigureSerilog();

            // add services
            builder.Services.ConfigureDependencies(builder.Configuration);
            builder.Services.ConfigureJwtAuthentication(builder.Configuration);
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureCors(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            // Register services

            // add controllers
            builder.Services.AddControllers();

            // add memory cache
            builder.Services.AddMemoryCache();
        }

        private static async Task ConfigureApplication(
        WebApplication app,
        CancellationToken cancellationToken
        )
        {
            // configure the global exception handler
            app.UseGlobalExceptionHandler();
            // app.UseHttpsRedirection();

            // Configure CORS based on environment
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("DevelopmentCorsPolicy");
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseCors("ProductionCorsPolicy");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            await app.RunAsync(cancellationToken);
        }
    }
}