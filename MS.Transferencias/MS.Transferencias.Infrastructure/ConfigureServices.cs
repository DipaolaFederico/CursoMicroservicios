using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Transferencias.Application.Common.Interfaces;
using MS.Transferencias.Infrastructure.Database;
using MS.Transferencias.Infrastructure.ExternalServices;
using MS.Transferencias.Infrastructure.Handlers;
using MS.Transferencias.Infrastructure.Health;
using MS.Transferencias.Infrastructure.Logging;
using MS.Transferencias.Infrastructure.Repositories;
using Polly;
using Polly.Timeout;
using Serilog;
using Serilog.Filters;

namespace MS.Transferencias.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder loggingBuilder)
        {
            services.AddTransient(typeof(IApplicationDbContext), typeof(ApplicationDbContext));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IClientService), typeof(ClientService));
            services.AddTransient<LoggingHandler>();

            services.AddMemoryCache();

            services.Configure<SerilogOptions>(configuration.GetSection(nameof(SerilogOptions)));
            services.Configure<ClientServiceOptions>(configuration.GetSection(nameof(ClientServiceOptions)));

            var serilogOptions = services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<SerilogOptions>>();
            var clientServiceOptions = services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<ClientServiceOptions>>();

            AddLogging(loggingBuilder, serilogOptions);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("ApiDatabase")));

            services.AddHttpContextAccessor();

            Random jitterer = new Random();

            services.AddHttpClient("ClientService", client =>
            {
                client.BaseAddress = new Uri(clientServiceOptions.CurrentValue.BaseUrl);
            })
            .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(5,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000))))
            .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
                3,
                TimeSpan.FromSeconds(15)
            ))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1))
            .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler();
                })
            .AddHttpMessageHandler<LoggingHandler>();

            services.AddHealthChecks()
               .AddCheck<HealthCheckClientService>("ClientService")
               .AddDbContextCheck<ApplicationDbContext>("Database");

            return services;
        }

        private static void AddLogging(ILoggingBuilder loggingBuilder, IOptionsMonitor<SerilogOptions> options)
        {
            var withExternalProperty = Matching.WithProperty("External");
            var withApiRequestResponseProperty = Matching.WithProperty("ApiRequestResponse");

            var logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Is(options.CurrentValue.GetLogLevel())
                .WriteTo.Logger(lc =>
                    lc.Enrich.FromLogContext()
                      .Filter.ByExcluding(e => withExternalProperty(e) || withApiRequestResponseProperty(e))
                      .WriteTo.File(options.CurrentValue.FilePath, rollingInterval: RollingInterval.Day,
                                    outputTemplate: options.CurrentValue.OutputTemplate))
                .WriteTo.Logger(lc =>
                    lc.Enrich.FromLogContext()
                      .Filter.ByIncludingOnly(e => withExternalProperty(e))
                      .WriteTo.File(options.CurrentValue.FilePathExternal, rollingInterval: RollingInterval.Day,
                                    outputTemplate: options.CurrentValue.OutputTemplate))
                .WriteTo.Logger(lc =>
                    lc.Enrich.FromLogContext()
                      .Filter.ByIncludingOnly(e => withApiRequestResponseProperty(e))
                      .WriteTo.File(options.CurrentValue.FilePathApiRequestResponse, rollingInterval: RollingInterval.Day,
                                    outputTemplate: options.CurrentValue.OutputTemplate))
                .Enrich.WithCorrelationIdHeader("correlation-id")
                .Enrich.WithMasking()
                .CreateLogger();

            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger);
        }


        public static IApplicationBuilder AddMigrations(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                logger.LogInformation("Iniciando proceso de migración de BD");

                dbContext.Database.Migrate();

            }
            catch (Exception ex)
            {
                logger.LogError("Error al ratar de generar la migración");
            }

            return app;
        }
    }
}
