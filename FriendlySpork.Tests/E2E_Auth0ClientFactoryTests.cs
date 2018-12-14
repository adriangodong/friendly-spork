using Auth0.ManagementApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace FriendlySpork.Tests
{
    [TestClass]
    public class E2E_Auth0ClientFactoryTests
    {
        [TestMethod]
        public async Task GetManagementClientAsync_ShouldSucceed()
        {
            var auth0FactoryOptions = new Auth0FactoryOptions
            {
                Domain = Environment.GetEnvironmentVariable("E2E_Domain"),
                ClientId = Environment.GetEnvironmentVariable("E2E_ClientId"),
                ClientSecret = Environment.GetEnvironmentVariable("E2E_ClientSecret"),
                ReuseAccessToken = false
            };

            if (auth0FactoryOptions.Domain == null
                || auth0FactoryOptions.ClientId == null
                || auth0FactoryOptions.ClientSecret == null)
            {
                Assert.Fail("Environment variables are not configured correctly.");
            }

            var accessTokenFactory = new Auth0AccessTokenFactory(auth0FactoryOptions);
            var clientFactory = new Auth0ClientFactory(auth0FactoryOptions, accessTokenFactory);
            var client = await clientFactory.GetManagementApiClientAsync();

            var getUsersRequest = new GetUsersRequest();
            var users = await client.Users.GetAllAsync(getUsersRequest);

            Assert.IsNotNull(users);
            Assert.AreEqual(0, users.Count);
        }
    }
}
