using FriendlySpork.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FriendlySpork
{
    public sealed class Auth0AccessTokenFactory : IAuth0AccessTokenFactory, IDisposable
    {
        private readonly string domain;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly bool reuseAccessToken;

        private readonly HttpClient httpClient;
        private readonly bool shouldDisposeHttpClient;

        private TokenResponse lastToken = null;
        private DateTimeOffset? lastTokenExpiration = null;

        public Auth0AccessTokenFactory(
            string domain,
            string clientId,
            string clientSecret,
            bool reuseAccessToken,
            HttpClient httpClient = null)
        {
            this.domain = domain;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.reuseAccessToken = reuseAccessToken;

            if (httpClient == null)
            {
                httpClient = new HttpClient();
                shouldDisposeHttpClient = true;
            }
            this.httpClient = httpClient;
        }

        public Auth0AccessTokenFactory(
            Auth0FactoryOptions options,
            HttpClient httpClient = null) :
            this(options.Domain, options.ClientId, options.ClientSecret, options.ReuseAccessToken, httpClient)
        {
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // If access token is not reused, always request a new one
            if (!reuseAccessToken)
            {
                var token = await RequestTokenAsync();
                return token.access_token;
            }

            // Refresh access token if expired or has never been requested
            if (lastToken == null || lastTokenExpiration == null || DateTimeOffset.UtcNow > lastTokenExpiration)
            {
                lastToken = await RequestTokenAsync();
                lastTokenExpiration = DateTimeOffset.UtcNow.AddSeconds(lastToken.expires_in);
            }

            return lastToken.access_token;
        }

        public string GetAccessToken()
        {
            string accessToken = null;

            Task.Run(async () =>
            {
                accessToken = await GetAccessTokenAsync();
            }).Wait();

            return accessToken;
        }

        private async Task<TokenResponse> RequestTokenAsync()
        {
            var requestUri = $"https://{domain}/oauth/token";
            var requestContent = new ApplicationJsonContent(new TokenRequest
            {
                grant_type = "client_credentials",
                client_id = clientId,
                client_secret = clientSecret,
                audience = $"https://{domain}/api/v2/"
            });

            var responseMessage = await httpClient.PostAsync(requestUri, requestContent);

            return JsonConvert.DeserializeObject<TokenResponse>(await responseMessage.Content.ReadAsStringAsync());
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
