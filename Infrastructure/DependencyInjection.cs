using Application.Abstractions.Authentication;
using Domain.Abstractions.Contracts;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Infrastructure.Database;
using Infrastructure.EmailService;
using Infrastructure.Helpers.EmailVerificationLink;
using Infrastructure.Options;
using MediatR;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.Options;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration Configuration) =>

            services
                .AddService()
                .AddHttpContextAccessor()
                .AddDataBaseService(Configuration)
                .AddAuthenticationInternal(Configuration)
                .AddEmailService(Configuration);
        

        static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddScoped<IEmailVerfication, EmailVerfication>();
            services.AddScoped<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();
            services.ConfigureOptions<EmailSetUp>();
            return services;
        }

        static IServiceCollection AddDataBaseService(this IServiceCollection services , IConfiguration configuration) 
        {
           var ConnectionString = configuration.GetConnectionString("DefaultConnection");
               services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(ConnectionString).
                        UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).
                        LogTo(log => Debug.WriteLine(log), LogLevel.Information).
                        EnableSensitiveDataLogging());
               return services;
        }

        static IServiceCollection AddAuthenticationInternal(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            return services;
        }

        static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }

 
    }
}
