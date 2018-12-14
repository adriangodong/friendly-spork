using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace FriendlySpork.Tests
{
    [TestClass]
    public class Auth0ClientFactoryTests
    {
        private string domain;
        private Mock<IAuth0AccessTokenFactory> mockAccessTokenFactory;
        private Auth0ClientFactory clientFactory;

        [TestInitialize]
        public void Initialize()
        {
            domain = Guid.NewGuid().ToString();
            mockAccessTokenFactory = new Mock<IAuth0AccessTokenFactory>();

            clientFactory = new Auth0ClientFactory(
                domain,
                mockAccessTokenFactory.Object);
        }

        [TestMethod]
        public async Task GetManagementApiClientAsync_CallsTokenFactoryAndReturnClientObject()
        {
            // Arrange
            mockAccessTokenFactory
                .Setup(mock => mock.GetAccessTokenAsync())
                .ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            var client = await clientFactory.GetManagementApiClientAsync();

            // Assert
            Assert.IsNotNull(client);
        }

        [TestMethod]
        public void GetManagementApiClient_CallsTokenFactoryAndReturnClientObject()
        {
            // Arrange
            mockAccessTokenFactory
                .Setup(mock => mock.GetAccessTokenAsync())
                .ReturnsAsync(Guid.NewGuid().ToString());

            // Act
            var client = clientFactory.GetManagementApiClient();

            // Assert
            Assert.IsNotNull(client);
        }
    }
}
