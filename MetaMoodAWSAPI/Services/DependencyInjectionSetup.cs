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
                    "server=ls-249ad327ce44da9463f737ece7b6de0d2b258dc1.cjdpzq5pew4s.us-east-2.rds.amazonaws.com;port=3306;user=root;password=lsdmCS4243;database=meta_mood",
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")
                );
            }
            );
            return services;
        }
    }
}
