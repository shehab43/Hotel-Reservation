using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) =>
            services
            .AddMediatRservices()
            .AddSFluentValidationservices();

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


    };
}
