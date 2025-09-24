using Application.Abstractions.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration Configuration,IHostBuilder hostBuilder) =>
            services
            .AddMediatRservices()
            .AddSFluentValidationservices()
            .RegisterService()
            .AddSerilogService(Configuration,hostBuilder);

        static IServiceCollection AddMediatRservices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;

        }
        static IServiceCollection AddSFluentValidationservices(this IServiceCollection services)
        {
            services
             .AddFluentValidationAutoValidation()
             .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
        static IServiceCollection AddSerilogService(this IServiceCollection services, IConfiguration configuration, IHostBuilder hostBuilder) 
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            var sinkOpts = new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            };
            
            var columnOpts = new ColumnOptions();
            columnOpts.Store.Add(StandardColumn.LogEvent);
            columnOpts.PrimaryKey = columnOpts.TimeStamp;
            columnOpts.TimeStamp.NonClusteredIndex = true;
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: sinkOpts,
                    columnOptions: columnOpts
                )
                .CreateLogger();
                
            hostBuilder.UseSerilog();
            return services;
        }

        static IServiceCollection RegisterService(this IServiceCollection services) 
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    };
}
