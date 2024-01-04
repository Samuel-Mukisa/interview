using Ishop.Application.Services;

//using Ishop.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.IoC
{
    public  static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
<<<<<<< HEAD
            services.AddTransient<ICustomerRegistrationService, CustomerRegistrationService>();
=======
           // services.AddTransient<ICustomerRegistrationService, CustomerRegistrationService>();
>>>>>>> orders
            return services;
        }

    }
}
