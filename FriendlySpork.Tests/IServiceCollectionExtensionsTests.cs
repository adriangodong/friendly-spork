using Auth0.ManagementApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace FriendlySpork.Tests
{
    [TestClass]
    public class IServiceCollectionExtensionsTests
    {
        private Mock<IServiceCollection> mockServices;

        [TestInitialize]
        public void Initialize()
        {
            mockServices = new Mock<IServiceCollection>();
        }

        [TestMethod]
        public void AddAuth0ManagementApi_ShouldCallConfigureOptionsCallback()
        {
            // Arrange
            var callbackExecuted = false;
            Auth0ClientFactoryOptions callbackOptions = null;

            // Act
            mockServices.Object.AddAuth0ManagementApi((options) =>
            {
                callbackExecuted = true;
                callbackOptions = options;
            });

            // Assert
            Assert.IsTrue(callbackExecuted);
            Assert.IsNotNull(callbackOptions);
        }

        [TestMethod]
        public void AddAuth0ManagementApi_ShouldAddIAuth0ClientFactoryAsSingleton()
        {
            // Arrange
            var mockConfigureOptionsCallback = new Mock<Action<Auth0ClientFactoryOptions>>();
            var singletonFactoryAdded = false;

            mockServices
                .Setup(mock => mock.Add(It.IsAny<ServiceDescriptor>()))
                .Callback((ServiceDescriptor serviceDescriptor) =>
                {
                    if (serviceDescriptor.Lifetime == ServiceLifetime.Singleton
                        && serviceDescriptor.ServiceType == typeof(IAuth0ClientFactory))
                    {
                        singletonFactoryAdded = true;
                    }
                });

            // Act
            mockServices.Object.AddAuth0ManagementApi(mockConfigureOptionsCallback.Object);

            // Assert
            Assert.IsTrue(singletonFactoryAdded);
        }

        [TestMethod]
        public void AddAuth0ManagementApi_ShouldAddIManagementApiClientAsTransient()
        {
            // Arrange
            var mockConfigureOptionsCallback = new Mock<Action<Auth0ClientFactoryOptions>>();
            var transientClientAdded = false;

            mockServices
                .Setup(mock => mock.Add(It.IsAny<ServiceDescriptor>()))
                .Callback((ServiceDescriptor serviceDescriptor) =>
                {
                    if (serviceDescriptor.Lifetime == ServiceLifetime.Transient
                        && serviceDescriptor.ServiceType == typeof(IManagementApiClient))
                    {
                        transientClientAdded = true;
                    }
                });

            // Act
            mockServices.Object.AddAuth0ManagementApi(mockConfigureOptionsCallback.Object);

            // Assert
            Assert.IsTrue(transientClientAdded);
        }

        [TestMethod]
        public void CreateManagementApi_CallsFactoryMethod()
        {
            // Arrange
            var mockClient = new Mock<IManagementApiClient>();

            var mockFactory = new Mock<IAuth0ClientFactory>();
            mockFactory
                .Setup(mock => mock.GetManagementApiClient())
                .Returns(mockClient.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(mock => mock.GetService(typeof(IAuth0ClientFactory)))
                .Returns(mockFactory.Object);

            // Act
            var client = IServiceCollectionExtensions.CreateManagementApi(mockServiceProvider.Object);

            // Assert
            Assert.IsNotNull(client);
            Assert.AreEqual(mockClient.Object, client);
        }
    }
}
