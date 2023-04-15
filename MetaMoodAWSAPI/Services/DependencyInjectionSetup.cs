using AutoMapper;
using MetaMoodAWSAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MetaMoodAWSAPI.Services
{
    public static class DependencyInjectionSetup
    {

        /// <summary>
        /// This is an extension method for the IServiceCollection interface that allows for services to be registered
        /// for DI. In this case, it adds a database context and mapper as services to the IServiceCollection.
        /// </summary>
        /// <example>
        /// This method can either be called directly on an IServiceCollection object or it can be chained
        /// with other methods. Here is an example of method chaining that can be done because it is an extension method:
        /// <code>
        /// _serviceCollection.RegisterSwagger().RegisterServices();
        /// </code>
        /// </example>
        /// <remarks>
        /// The ConnectionString environment variable comes either from launchSettings.json for the development environment
        /// or from AWS Lambda for the production environment.
        /// </remarks>
        /// <param name="services">the collection of services</param>
        /// <returns>IServiceCollection with all services registered</returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services, string Conn)
        {
            services.AddEntityFrameworkMySql().AddDbContext<MetaMoodContext>(options =>
            {
                options.UseMySql
                (
                    Conn,
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
