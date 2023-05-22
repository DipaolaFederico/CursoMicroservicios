using Microsoft.AspNetCore.Builder;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Clientes.Application.Common.Interfaces;
using MS.Clientes.Infrastructure.Database;
using MS.Clientes.Infrastructure.Logging;
using MS.Clientes.Infrastructure.Repositories;
using Serilog;
using Serilog.Filters;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using MS.Clientes.Infrastructure.Producers;

namespace MS.Clientes.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder loggingBuilder)
        {
            services.AddTransient(typeof(IKafkaProducer), typeof(KafkaProducer));
            services.AddTransient(typeof(IApplicationDbContext), typeof(ApplicationDbContext));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
     
            services.AddMemoryCache();

            services.AddKafka(kafka =>
            {
                kafka.AddCluster(cluster =>
                {
                    string topicName = "Clientes";
                    cluster.WithBrokers(new[] { "localhost:9092" })
                        .CreateTopicIfNotExists(topicName, 1, 1)
                        .AddProducer("publish-client", producer =>
                        {
                            producer.DefaultTopic(topicName)
                                .AddMiddlewares(middlewares: producerMiddlewareConfigurationBuilder =>
                                {
                                    producerMiddlewareConfigurationBuilder.AddSerializer<JsonCoreSerializer>();
                                });
                        });
                });
            });


            services.Configure<SerilogOptions>(configuration.GetSection(nameof(SerilogOptions)));

            var serilogOptions = services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<SerilogOptions>>();

            AddLogging(loggingBuilder, serilogOptions);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("ApiDatabase")));

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
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
