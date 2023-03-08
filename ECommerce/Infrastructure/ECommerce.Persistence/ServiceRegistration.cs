using Microsoft.EntityFrameworkCore;
using ECommerce.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Persistence.Configurations;
using ECommerce.Application.Repositories.Abstracts;
using ECommerce.Persistence.Repositories;
using ECommerce.Persistence.Repositories.File;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Persistence.Services;
using ECommerce.Application.Abstractions.Services.Authentications;

namespace ECommerce.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            //services.AddSingleton<IProductService, ProductService>();
            services.AddDbContext<ECommerceDbContext>(options => options.UseNpgsql(Configuration.ConnectionString),ServiceLifetime.Singleton);

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<ECommerceDbContext>();

            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddSingleton<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<IProductImageReadRepository, ProductImageReadRepository>();
            services.AddScoped<IProductImageWriteRepository, ProductImageWriteRepository>();

            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();

            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();

            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();    
            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();
            services.AddScoped<IBasketService,BasketService>();
        }
    }
}
