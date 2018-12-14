using FriendlySpork.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FriendlySpork.Tests
{
    [TestClass]
    public class Auth0AccessTokenFactoryTests
    {
        private Auth0FactoryOptions options;
        private MockHttpMessageHandler mockHttpMessageHandler;
        private Auth0AccessTokenFactory accessTokenFactory;

        private HttpResponseMessage CreateResponseMessage(string accessToken, int expireIn)
        {
            return new HttpResponseMessage()
            {
                Content = new ApplicationJsonContent(new TokenResponse
                {
                    access_token = accessToken,
                    expires_in = expireIn
                })
            };
        }

        [TestInitialize]
        public void Initialize()
        {
            options = new Auth0FactoryOptions()
            {
                Domain = Guid.NewGuid().ToString(),
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
                ReuseAccessToken = false
            };

            mockHttpMessageHandler = new MockHttpMessageHandler();
            accessTokenFactory = null; // Reset factory
        }

        [TestMethod]
        public async Task GetAccessTokenAsync_ProcessRequestResponseCorrectly()
        {
            // Arrange
            var accessToken = Guid.NewGuid().ToString();

            mockHttpMessageHandler
                .When(HttpMethod.Post, $"https://{options.Domain}/oauth/token")
                .Respond(request => CreateResponseMessage(accessToken, 0));

            accessTokenFactory = new Auth0AccessTokenFactory(options, mockHttpMessageHandler.ToHttpClient());

            // Act
            var actualAccessToken = await accessTokenFactory.GetAccessTokenAsync();

            // Assert
            mockHttpMessageHandler.VerifyNoOutstandingRequest();
            Assert.AreEqual(accessToken, actualAccessToken);
        }

        [TestMethod]
        public async Task GetAccessTokenAsync_WithTokenReuse_ShouldOnlyRequestOnce()
        {
            // Arrange
            mockHttpMessageHandler
                .Expect(HttpMethod.Post, "*")
                .Respond(request => CreateResponseMessage(string.Empty, 3600));

            options.ReuseAccessToken = true;

            accessTokenFactory = new Auth0AccessTokenFactory(options, mockHttpMessageHandler.ToHttpClient());

            // Act
            await accessTokenFactory.GetAccessTokenAsync();
            await accessTokenFactory.GetAccessTokenAsync();

            // Assert
            mockHttpMessageHandler.VerifyNoOutstandingRequest();
        }

        [TestMethod]
        public async Task GetAccessTokenAsync_WithTokenReuseAndExpired_ShouldRequestTwice()
        {
            // Arrange
            mockHttpMessageHandler
                .Expect(HttpMethod.Post, "*")
                .Respond(request => CreateResponseMessage(string.Empty, -3600));

            mockHttpMessageHandler
                .Expect(HttpMethod.Post, "*")
                .Respond(request => CreateResponseMessage(string.Empty, -3600));

            options.ReuseAccessToken = true;

            accessTokenFactory = new Auth0AccessTokenFactory(options, mockHttpMessageHandler.ToHttpClient());

            // Act
            await accessTokenFactory.GetAccessTokenAsync();
            await accessTokenFactory.GetAccessTokenAsync();

            // Assert
            mockHttpMessageHandler.VerifyNoOutstandingRequest();
        }

        [TestMethod]
        public void GetAccessToken_ProcessRequestResponseCorrectly()
        {
            // Arrange
            var accessToken = Guid.NewGuid().ToString();

            mockHttpMessageHandler
                .When(HttpMethod.Post, $"https://{options.Domain}/oauth/token")
                .Respond(request => CreateResponseMessage(accessToken, 0));

            accessTokenFactory = new Auth0AccessTokenFactory(options, mockHttpMessageHandler.ToHttpClient());

            // Act
            var actualAccessToken = accessTokenFactory.GetAccessToken();

            // Assert
            mockHttpMessageHandler.VerifyNoOutstandingRequest();
            Assert.AreEqual(accessToken, actualAccessToken);
        }
    }
}
