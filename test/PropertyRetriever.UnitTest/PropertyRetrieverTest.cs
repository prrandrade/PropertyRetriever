namespace PropertyRetriever.UnitTest
{
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using static PropertyRetriever;

    public class PropertyRetrieverTest
    {
        [Fact]
        public void AddLocalEnvironment()
        {
            // arrange
            var services = new ServiceCollection();

            // act
            var t = RetrieveProperty<string>("property", "-p", "--property")



            services.AddLocalEnvironment();
            var container = services.BuildServiceProvider();
            var service1 = container.GetService<ILocalEnvironment>();
            var service2 = container.GetService<ILocalEnvironment>();

            // assert
            Assert.NotNull(container.GetService<ILocalEnvironment>());
            Assert.Equal(service1, service2);
        }

        [Fact]
        public void AddPropertyRetriever()
        {
            // arrange
            var services = new ServiceCollection();

            // act
            services.AddPropertyRetriever();
            var container = services.BuildServiceProvider();
            var service1 = container.GetService<IPropertyRetriever>();
            var service2 = container.GetService<IPropertyRetriever>();

            // assert
            Assert.NotNull(container.GetService<IPropertyRetriever>());
            Assert.Equal(service1, service2);
        }
    }
}
