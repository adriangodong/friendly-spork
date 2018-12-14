using Auth0.ManagementApi;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FriendlySpork
{
    public sealed class Auth0ClientFactory : IAuth0ClientFactory, IDisposable
    {
        private readonly string domain;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly HttpClient httpClient;
        private readonly bool shouldDisposeHttpClient;

        public Auth0ClientFactory(
            string domain,
            string clientId,
            string clientSecret,
            HttpClient httpClient = null)
        {
            this.domain = domain;
            this.clientId = clientId;
            this.clientSecret = clientSecret;

            if (httpClient == null)
            {
                httpClient = new HttpClient();
                shouldDisposeHttpClient = true;
            }
            this.httpClient = httpClient;
        }

        public Auth0ClientFactory(
            Auth0ClientFactoryOptions options,
            HttpClient httpClient = null) :
            this(options.Domain, options.ClientId, options.ClientSecret, httpClient)
        {
        }

        public async Task<IManagementApiClient> GetManagementApiClientAsync()
        {
            return new ManagementApiClient(
                await GetAccessToken(),
                domain);
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

        private async Task<string> GetAccessToken()
        {
            var requestUri = $"https://{domain}/oauth/token";
            var requestBody = JsonConvert.SerializeObject(new
            {
                grant_type = "client_credentials",
                client_id = clientId,
                client_secret = clientSecret,
                audience = $"https://{domain}/api/v2/"
            });
            var requestBodyBytes = Encoding.UTF8.GetBytes(requestBody);
            var requestContent = new ByteArrayContent(requestBodyBytes);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync(requestUri, requestContent);

            var responseBodyBytes = await response.Content.ReadAsByteArrayAsync();
            var responseBodyString = Encoding.UTF8.GetString(responseBodyBytes);
            var responseBody = JsonConvert.DeserializeAnonymousType(
                responseBodyString,
                new { access_token = "", token_type = "", expires_in = 0 });

            // TODO: optionally cache (and invalidate) access token
            return responseBody.access_token;
        }

        public void Dispose()
        {
            if (shouldDisposeHttpClient)
            {
                httpClient.Dispose();
            }
        }
    }
}
