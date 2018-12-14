using Auth0.ManagementApi;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FriendlySpork
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth0ManagementApi(
            this IServiceCollection services,
            Action<Auth0ClientFactoryOptions> configureOptions)
        {
            var options = new Auth0ClientFactoryOptions();
            configureOptions(options);

            IAuth0ClientFactory auth0ClientFactory = new Auth0ClientFactory(
                options.Domain,
                options.ClientId,
                options.ClientSecret);

            services.AddSingleton<IAuth0ClientFactory>(auth0ClientFactory);
            services.AddTransient(CreateManagementApi);

            return services;
        }

        public static Func<IServiceProvider, IManagementApiClient> CreateManagementApi =
            (serviceProvider) => serviceProvider
                .GetRequiredService<IAuth0ClientFactory>()
                .GetManagementApiClient();
    }
}
