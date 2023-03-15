using AutoMapper;
using MetaMoodAWSAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MetaMoodAWSAPI.Services
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddEntityFrameworkMySql().AddDbContext<MetaMoodContext>(options =>
                {
                options.UseMySql
                    (
                    // NOTE about connection string: for development, change environment variable in launchSettings.json
                    // for production, change environment variable in aws-lambda-tools-defaults.json
                        System.Environment.GetEnvironmentVariable("ConnectionString"),
                        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")
                    );
                }
            );
            var mapperConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AllMappersProfile());
                }
            );

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
