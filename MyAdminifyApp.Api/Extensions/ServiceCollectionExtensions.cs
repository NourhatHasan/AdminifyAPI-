using MyAdminifyApp.Application.Interfaces;
using MyAdminifyApp.Application.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;


namespace MyAdminifyApp.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddScoped<IProdutService, ProductService>();
            services.AddSingleton<IBlobService, BlobService>();
            services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.CommandTimeout(180)
                ));
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1024 * 1024 * 10; 
            });
            services.AddMemoryCache();

            return services;
        }
    }
}
