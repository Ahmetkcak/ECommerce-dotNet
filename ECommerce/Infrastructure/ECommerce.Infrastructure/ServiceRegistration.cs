using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Services.Configurations;
using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Services.Configurations;
using ECommerce.Infrastructure.Services.Storage;
using ECommerce.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IApplicationService, ApplicationService>();
        }

        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage 
        {
            services.AddScoped<IStorage, T>();
        }
    }
}
