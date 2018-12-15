using Auth0.ManagementApi;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FriendlySpork
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Auth0.ManagementApi.IManagementApiClient and required types into the service collection object.
        /// </summary>
        /// <param name="services">Current service collection instance.</param>
        /// <param name="configureOptions">Extension point to provide options to the factory.</param>
        /// <returns></returns>
        public static IServiceCollection AddAuth0ManagementApi(
            this IServiceCollection services,
            Action<Auth0FactoryOptions> configureOptions)
        {
            var options = new Auth0FactoryOptions();
            configureOptions(options);

            services.AddSingleton(options);
            services.AddSingleton<IAuth0AccessTokenFactory>(
                serviceProvider => new Auth0AccessTokenFactory(options));
            services.AddSingleton<IAuth0ClientFactory>(
                serviceProvider => new Auth0ClientFactory(
                    options,
                    serviceProvider.GetRequiredService<IAuth0AccessTokenFactory>()));
            services.AddTransient(CreateManagementApi);

            return services;
        }

        /// <summary>
        /// Factory method to create a new IManagementApiClient instance. Use AddAuth0ManagementApi() instead.
        /// </summary>
        public static Func<IServiceProvider, IManagementApiClient> CreateManagementApi =
            (serviceProvider) => serviceProvider
                .GetRequiredService<IAuth0ClientFactory>()
                .GetManagementApiClient();
    }
}
