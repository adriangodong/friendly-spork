using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FriendlySpork.Tests
{
    [TestClass]
    public class Auth0ClientFactoryTests
    {
        private string domain;
        private MockHttpMessageHandler mockHttpMessageHandler;
        private Auth0ClientFactory auth0ClientFactory;

        [TestInitialize]
        public void Initialize()
        {
            domain = Guid.NewGuid().ToString();

            mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler.AutoFlush = true;

            auth0ClientFactory = new Auth0ClientFactory(
                domain,
                null,
                null,
                mockHttpMessageHandler.ToHttpClient());
        }

        [TestMethod]
        public async Task GetManagementApiClientAsync_UsesHttpClient()
        {
            // Arrange
            mockHttpMessageHandler
                .When(HttpMethod.Post, $"https://{domain}/oauth/token")
                .Respond(request => new HttpResponseMessage()
                {
                    Content = new StringContent("{ 'access_token': '', 'token_type': '', expires_in: 0 }")
                });

            // Act
            var client = await auth0ClientFactory.GetManagementApiClientAsync();

            // Assert
            mockHttpMessageHandler.VerifyNoOutstandingRequest();
        }
    }
}
