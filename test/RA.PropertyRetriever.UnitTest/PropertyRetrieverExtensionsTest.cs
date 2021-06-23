namespace RA.PropertyRetriever.UnitTest
{
    using Microsoft.Extensions.DependencyInjection;
    using RA.PropertyRetriever;
    using RA.PropertyRetriever.Services.Interfaces;
    using Xunit;

    public class PropertyRetrieverExtensionsTest
    {
        [Fact]
        public void AddLocalEnvironment()
        {
            // arrange
            var services = new ServiceCollection();

            // act
            services.AddLocalEnvironmentService();
            var container = services.BuildServiceProvider();
            var service1 = container.GetService<ILocalEnvironmentService>();
            var service2 = container.GetService<ILocalEnvironmentService>();

            // assert
            Assert.NotNull(container.GetService<ILocalEnvironmentService>());
            Assert.Equal(service1, service2);
        }

        [Fact]
        public void AddPropertyRetriever()
        {
            // arrange
            var services = new ServiceCollection();

            // act
            services.AddPropertyRetrieverService();
            var container = services.BuildServiceProvider();
            var service1 = container.GetService<IPropertyRetrieverService>();
            var service2 = container.GetService<IPropertyRetrieverService>();

            // assert
            Assert.NotNull(container.GetService<IPropertyRetrieverService>());
            Assert.Equal(service1, service2);
        }
    }
}
