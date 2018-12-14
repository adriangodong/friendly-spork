using Auth0.ManagementApi;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FriendlySpork
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth0ManagementApi(
            this IServiceCollection services,
            Action<Auth0FactoryOptions> configureOptions)
        {
            var options = new Auth0FactoryOptions();
            configureOptions(options);

            services.AddSingleton(options);
            services.AddSingleton<IAuth0AccessTokenFactory, Auth0AccessTokenFactory>();
            services.AddSingleton<IAuth0ClientFactory, Auth0ClientFactory>();
            services.AddTransient(CreateManagementApi);

            return services;
        }

        public static Func<IServiceProvider, IManagementApiClient> CreateManagementApi =
            (serviceProvider) => serviceProvider
                .GetRequiredService<IAuth0ClientFactory>()
                .GetManagementApiClient();
    }
}
