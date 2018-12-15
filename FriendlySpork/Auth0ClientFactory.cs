using Auth0.ManagementApi;
using System.Threading.Tasks;

namespace FriendlySpork
{
    public sealed class Auth0ClientFactory : IAuth0ClientFactory
    {
        private readonly string domain;
        private readonly IAuth0AccessTokenFactory accessTokenFactory;

        public Auth0ClientFactory(
            string domain,
            IAuth0AccessTokenFactory auth0AccessTokenFactory)
        {
            this.domain = domain;
            this.accessTokenFactory = auth0AccessTokenFactory;
        }

        public Auth0ClientFactory(
            Auth0FactoryOptions options,
            IAuth0AccessTokenFactory auth0AccessTokenFactory) :
            this(options.Domain, auth0AccessTokenFactory)
        {
        }

        public async Task<IManagementApiClient> GetManagementApiClientAsync()
        {
            return new ManagementApiClient(await accessTokenFactory.GetAccessTokenAsync(), domain);
        }

        public IManagementApiClient GetManagementApiClient()
        {
            IManagementApiClient auth0ManagementApiClient = null;

            Task.Run(async () =>
            {
                auth0ManagementApiClient = await GetManagementApiClientAsync();
            }).Wait();

            return auth0ManagementApiClient;
        }
    }
}
